using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Common.Utils;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public CategoriesAndProducts CategoriesAndProducts { get; set; } = new();

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync()
        {
            var categoryTask = _apiClient.GetAsync("/categories?page=1&size=5");
            var productTask = _apiClient.GetAsync("/products?page=1&size=5");

            await Task.WhenAll(categoryTask, productTask);

            var categoryResult = categoryTask.Result;
            var productResult = productTask.Result;

            CategoriesAndProducts.Categories = GetItemsFromResponse<Category>(categoryResult);
            CategoriesAndProducts.Products = GetItemsFromResponse<Product>(productResult);
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

    public class CategoriesAndProducts
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}