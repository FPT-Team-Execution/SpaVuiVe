using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using SkincareProductSalesSystem.Common.Utils;
using SkincareProductSalesSystem.Services.Base;
using Solara.Main.Payload;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkincareProductSalesSystem.Services
{
    public interface IChatBotService
    {
        Task<IServiceResult> ChatWithBot(string message);
    }
    public class ChatBotService : IChatBotService
    {
        private readonly IConfiguration _configuration;
        public ChatBotService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IServiceResult> ChatWithBot(string prompt)
        {
            try
            {
                //promtp template
                string finalPrompt = $"Bạn đóng vai trò là chuyên gia tư vấn về chăm sóc da với tên là SpaBot. Bạn chỉ trả lời các câu hỏi liên quan đến da, nếu không biết thì nói 'Xin lỗi, tôi không biết về vấn đề này.' Hãy giữ phong cách chuyên nghiệp, rõ ràng, có cơ sở khoa học và các câu trả lời dễ hiểu, tự nhiên, không mang thiên hướng kỹ thuật. Câu hỏi: '{prompt}'";

                var url = $"{_configuration["AI:Url"]}?key={_configuration["AI:ApiKey"]}";
                var body =
                  new GeminiPrompt(finalPrompt);
                var response = await WebUtil.PostAsync(
                    url: url, data: body
                );

                if (!response.IsSuccessStatusCode)
                    return new ServiceResult(503, "Thực hiện yêu cầu thất bại");

                var content = await response.Content.ReadAsStringAsync();
                GeminiResponse geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(content)!;
                var data = geminiResponse!.Candidates.First().Content.Parts.First().Text;
                return new ServiceResult
                {
                    Status = 200,
                    Message = "Thành công",
                    Data = data
                };

            }
            catch (Exception e)
            {
                return new ServiceResult(503, "Thực hiện yêu cầu thất bại");
            }
          
        }
    }
}


#region Gemini Class Request
public class GeminiPrompt
{
    [JsonPropertyName("contents")]
    public List<Contents> Contents { get; set; }

    public GeminiPrompt(string prompt)
    {
        Contents = new List<Contents>
            {
                new Contents
                {
                    Parts = new List<Parts>
                    {
                        new Parts
                        {
                            Text = prompt
                        }
                    }
                }
            };
    }
}

public class Contents
{
    [JsonPropertyName("parts")]
    public List<Parts> Parts { get; set; }
}

public class Parts
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

#endregion

#region Gemini Class response
public class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; }
    [JsonPropertyName("usageMetadata")]
    public UsageMetadata UsageMetadata { get; set; }
    [JsonPropertyName("modelVersion")]
    public string ModelVersion { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content Content { get; set; }
    [JsonPropertyName("finishReason")]
    public string FinishReason { get; set; }
    [JsonPropertyName("avgLogprobs")]
    public double AvgLogprobs { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; }
    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class UsageMetadata
{
    [JsonPropertyName("promptTokenCount")]
    public int PromptTokenCount { get; set; }
    [JsonPropertyName("candidatesTokenCount")]
    public int CandidatesTokenCount { get; set; }
    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }
}
#endregion




