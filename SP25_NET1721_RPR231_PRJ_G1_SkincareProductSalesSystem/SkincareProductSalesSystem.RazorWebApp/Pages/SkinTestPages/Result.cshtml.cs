using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.RazorWebApp.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTestPages
{
    public class ResultModel : PageModel
    {
        private readonly ApiClient _apiClient;
        public ResultModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public SkinType SkinType { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            TempData.TryGetValue("SkinTypeResult", out object result);
            if (result == null)
            {
                return RedirectToPage("/SkinTestPages/Index");
            }
            SkinType = JsonConvert.DeserializeObject<SkinType>(result.ToString());
            return Page();
        }
    }
}
