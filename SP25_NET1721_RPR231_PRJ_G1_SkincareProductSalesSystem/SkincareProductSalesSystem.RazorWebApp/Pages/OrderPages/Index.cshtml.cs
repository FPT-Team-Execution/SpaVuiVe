using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Common.Utils;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.OrderPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } = 5;
        public List<Order> Orders { get; set; } = new();
        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _apiClient.GetAsync($"/orders?page={CurrentPage}&size={ItemsPerPage}");

                if (response.Data == null)
                {
                    // Handle error case - maybe set an error message
                    Orders = new List<Order>();
                    return Page(); // Return current page instead of redirecting
                }

                var paginatedData = JsonConvert.DeserializeObject<Paginate<Order>>(response.Data.ToString());

                // Update the page model properties
                TotalPages = paginatedData.TotalPages;
                ItemsPerPage = paginatedData.Size;
                CurrentPage = paginatedData.Page;
                Orders = paginatedData.Items;

                return Page(); // Return the current page instead of redirecting
            }
            catch (Exception ex)
            {
                // Handle exception - maybe set an error message
                Orders = new List<Order>();
                return Page(); // Return current page instead of redirecting
            }
        }

        public async Task<IActionResult> OnPostCancelAsync(string orderId)
        {
            var response = await _apiClient.GetAsync($"/orders/{orderId}");
            if(response.Data == null) return RedirectToPage();
            var order = JsonConvert.DeserializeObject<Order>(response.Data.ToString());
            order.Status = "Cancelled";
            var result = await _apiClient.PutAsync("/orders", order);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostViewDetailAsync(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToPage("/OrderPages/Index");
            }
            return RedirectToPage("/OrderDetailPages/Index", new { id = orderId });
        }
    }
}
