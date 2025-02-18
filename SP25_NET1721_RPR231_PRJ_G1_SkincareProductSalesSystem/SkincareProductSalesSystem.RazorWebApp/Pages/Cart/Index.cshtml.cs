using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;


namespace SkincareProductSalesSystem.RazorWebApp.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            IndextProductCartPage = new IndextProductCartPage();
        }

        public IndextProductCartPage IndextProductCartPage { get; set; }

        public async Task OnGetAsync()
        {
            var productCartResult = await _apiClient.GetAsync("/cart");

            if (productCartResult == null)
            {
                Console.WriteLine("API response is null.");
                return;
            }

            IndextProductCartPage.ProductCart = GetItemsFromResponse<ProductCart>(productCartResult)
                ?? new List<ProductCart>();
        }

        private List<T>? GetItemsFromResponse<T>(ServiceResult response)
        {
            if (response != null && response.Status == 200 && response.Data != null)
            {
                try
                {
                    var responseData = response.Data.ToString();
                    var result = JsonConvert.DeserializeObject<List<T>>(responseData!);
                    return result ?? new List<T>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Deserialization error: {ex.Message}");
                }
            }

            return new List<T>();
        }
    }

    public class IndextProductCartPage
    {
        public List<ProductCart>? ProductCart { get; set; }
    }
}
