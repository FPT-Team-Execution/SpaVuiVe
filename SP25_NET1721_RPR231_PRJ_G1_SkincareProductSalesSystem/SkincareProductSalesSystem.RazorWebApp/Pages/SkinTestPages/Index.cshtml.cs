using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Common.Utils;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;


namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTestPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;
        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }


        public List<SkinTest> SkinTest { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ServiceResult getSkinTestRes = await _apiClient.GetAsync("/skin-tests");
            SkinTest = GetItemsFromResponse<List<SkinTest>>(getSkinTestRes);
        }
        private T GetItemsFromResponse<T>(ServiceResult response) where T : new()
        {
            if (response.Status == 200 && response.Data != null)
            {
                var result = JsonConvert.DeserializeObject<T>(response.Data.ToString());
                return result;
            }

            // If response is invalid, return a new instance of T
            return new T();
        }

    }
}
