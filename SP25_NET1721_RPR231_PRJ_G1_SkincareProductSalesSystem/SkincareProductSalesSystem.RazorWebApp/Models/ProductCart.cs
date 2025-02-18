using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Models
{
    public class ProductCart
    {

        public required Product ProductInCart { get; set; }
        public int Quantity { get; set; }
    }
}
