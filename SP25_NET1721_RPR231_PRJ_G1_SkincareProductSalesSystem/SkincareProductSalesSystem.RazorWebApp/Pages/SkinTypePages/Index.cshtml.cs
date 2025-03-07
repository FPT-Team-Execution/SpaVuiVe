using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Protos.SkinTypesClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.Collections.Generic;
using System.Text.Json;
namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTypePages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public List<SkinType> SkinTypes = new();
        public async Task OnGet()
        {
            var response = await _apiClient.GetAsync("/skin-types");

            try
            {
                if (response.Data != null)
                    SkinTypes = JsonSerializer.Deserialize<List<SkinType>>(response.Data.ToString());
            }
            catch (Exception)
            {
                SkinTypes = [];
            }
    
        }
    }
}

