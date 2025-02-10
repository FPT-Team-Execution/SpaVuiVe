using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Protos.SkinTypesClient;
using SkincareProductSalesSystem.RazorWebApp.Models;
namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTypePages
{
    public class IndexModel : PageModel
    {
       // private readonly SkinTypes.SkinTypesClient _client;

        public IndexModel()
        {
            
        }
        public List<SkinType> SkinTypes = new();
        public async Task OnGet()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7000");
            SkinTypesService.SkinTypesServiceClient _client = new SkinTypesService.SkinTypesServiceClient(channel);
            var response =  await _client.GetAllAsync(new EmptyRequestProto());
            foreach (var skinType in response.Data)
            {
                SkinTypes.Add(new SkinType
                {
                    SkinTypeId = skinType.SkinTypeId,
                    Name = skinType.Name,
                    Description = skinType.Description,
                    Characteristics = skinType.Characteristics,
                    RecommendedIngredients = skinType.RecommendedIngredients,
                    AvoidIngredients = skinType.AvoidIngredients,
                    IsActive = skinType.IsActive,
                    CreatedAt = skinType.CreatedAt?.ToDateTime(),
                    UpdatedAt = skinType.UpdatedAt?.ToDateTime()
                });
            }
        }
    }
}

