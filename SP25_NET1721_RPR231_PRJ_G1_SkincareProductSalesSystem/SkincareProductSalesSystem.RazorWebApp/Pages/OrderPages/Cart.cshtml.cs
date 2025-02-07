using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;

namespace SpaVuiVe_FE.Pages.OrderPages
{
    public class CartModel : PageModel
    {
        private readonly ApiClient _apiClient;
        public CartModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
    }
}
