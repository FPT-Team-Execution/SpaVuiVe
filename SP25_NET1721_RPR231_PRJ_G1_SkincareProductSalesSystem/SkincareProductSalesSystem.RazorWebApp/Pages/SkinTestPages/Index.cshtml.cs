using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Common.Utils;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.Text;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTestPages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<SkinTest> SkinTest { get; set; } = new();
        public int QuestionCount { get; set; } = 0;
        public Dictionary<string, SkinTestOption> ChosenOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.TryGetValue("ChosenOptions", out var storedOptions))
            {
                var jsonString = Encoding.UTF8.GetString(storedOptions);
                ChosenOptions = JsonConvert.DeserializeObject<Dictionary<string, SkinTestOption>>(jsonString) ?? new();
            }

            ServiceResult getSkinTestRes = await _apiClient.GetAsync("/skin-tests");
            SkinTest = GetItemsFromResponse<List<SkinTest>>(getSkinTestRes);
            QuestionCount = SkinTest.Count;
        }

        private T GetItemsFromResponse<T>(ServiceResult response) where T : new()
        {
            if (response.Status == 200 && response.Data != null)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Data.ToString());
                return result ?? new T();
            }
            return new T();
        }

        public IActionResult OnPostSaveAnswer([FromBody] SaveAnswerBody data)
        {
            if (data != null)
            {
                // First, retrieve existing choices from session
                if (HttpContext.Session.TryGetValue("ChosenOptions", out var storedOptions))
                {
                    var jsonString = Encoding.UTF8.GetString(storedOptions);
                    ChosenOptions = JsonConvert.DeserializeObject<Dictionary<string, SkinTestOption>>(jsonString) ?? new();
                }

                // Update or add the new choice
                ChosenOptions[data.QuestionId] = data.Option;

                // Save back to session
                var serializedData = JsonConvert.SerializeObject(ChosenOptions);
                HttpContext.Session.SetString("ChosenOptions", serializedData);
            }
            return new JsonResult(new { success = true });
        }
    }

    public class SaveAnswerBody
    {
        public string QuestionId { get; set; }
        public SkinTestOption Option { get; set; }
    }
}