using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.OrderDetailPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty(SupportsGet = true)] // This is important for binding on GET requests
        public string Id { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var id = Id;
            var response = await _apiClient.GetMinh2Async<IPaginate<OrderDetail>>($"/order-details/{Id}?page=1&size=1000");
            if (response.Data == null)
            {
                return RedirectToPage("/OrderDetailPages/Index"); // Redirect to home page or appropriate error page
            }

            var orderDetailList = (Paginate<OrderDetail>)response.Data;
            OrderDetails = orderDetailList.Items;
            return Page(); // Return the current page instead of redirecting
        }
    }

}
