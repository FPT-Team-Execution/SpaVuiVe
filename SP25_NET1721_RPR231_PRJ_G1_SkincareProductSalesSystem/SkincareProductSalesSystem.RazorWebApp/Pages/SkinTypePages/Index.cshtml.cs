using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Protos.SkinTypesClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models;
namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTypePages
{
    public class IndexModel : PageModel
    {
       private readonly GrpcClient<SkinTypesService.SkinTypesServiceClient> _grpcClient;

        public IndexModel(GrpcClient<SkinTypesService.SkinTypesServiceClient> grpcClient)
        {
            _grpcClient = grpcClient;
        }
        public List<SkinType> SkinTypes = new();
        public async Task OnGet()
        {
            var response =  await _grpcClient.Client.GetAllAsync(new EmptyRequestProto());
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

