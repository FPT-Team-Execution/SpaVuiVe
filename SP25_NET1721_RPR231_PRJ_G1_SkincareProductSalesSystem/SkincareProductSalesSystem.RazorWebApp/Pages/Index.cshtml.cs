using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Common.Utils;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public List<Category> Categories { get; set; } = new List<Category>();

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync()
        {
            var result = await _apiClient.GetAsync("/categories?page=1&size=5");

            if (result.Status == 200 && result.Data != null)
            {
                var paginateCategories = JsonConvert.DeserializeObject<Paginate<Category>>(result.Data.ToString());
                Categories = paginateCategories.Items;
            }
            else
            {
                Categories = new List<Category>();
            }
        }
    }
}