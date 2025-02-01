using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var getSkinTestRes = await _apiClient.GetAsync("/skin-tests");
            SkinTest = getSkinTestRes.Data as List<SkinTest> ?? [];
        }
    }
}
