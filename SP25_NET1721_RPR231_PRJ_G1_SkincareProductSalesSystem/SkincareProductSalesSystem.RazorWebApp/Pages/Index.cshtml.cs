using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public ContentData contentData { get; set; } = new();

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync()
        {
            var categoryTask = _apiClient.GetAsync("/categories?page=1&size=5");
            var productTask = _apiClient.GetAsync("/products?page=1&size=8");
            var brandTask = _apiClient.GetAsync("/brands?page=1&size=5");

            await Task.WhenAll(categoryTask, productTask, brandTask);

            var categoryResult = categoryTask.Result;
            var productResult = productTask.Result;
            var brandResult = brandTask.Result;

            contentData.Categories = GetItemsFromResponse<Models.Category>(categoryResult);
            contentData.Products = GetItemsFromResponse<Product>(productResult);
            contentData.Brands = GetItemsFromResponse<Brand>(brandResult);
        }

        private List<T> GetItemsFromResponse<T>(ServiceResult response)
        {
            if (response.Status == 200 && response.Data != null)
            {
                var paginatedResult = JsonConvert.DeserializeObject<Paginate<T>>(response.Data.ToString());
                return paginatedResult.Items;
            }

            return new List<T>();
        }
    }

    public class ContentData
    {
        public List<Models.Category> Categories { get; set; } = new List<Models.Category>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Brand> Brands { get; set; } = new List<Brand>();
    }
}