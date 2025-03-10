using Azure;
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

        [BindProperty]
        public string SelectedProducts { get; set; }
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

        public async Task<IActionResult> OnPostAddToCart([FromForm] string productCartId, int quantity)
        {
            try
            {
                if (quantity > 0)
                {
                    var productCart = await _apiClient.GetMinhAsync<ProductCart>($"/cart/product/{productCartId}");
                    if (productCart != null && productCart.Status == 200 && productCart.Data != null)
                    {

                        if (productCart.Data is ProductCart)
                        {
                            var responseData = (ProductCart)productCart.Data;
                            if (responseData == null) throw new Exception();
                            var productCartResult = await _apiClient.PutAsync($"/cart/product?quantity={quantity}", responseData.Product);
                        }

                    }
                }
                else
                {
                    var productCart = await _apiClient.DeleteAsync($"/cart/product/{productCartId}");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedProducts))
                {
                    return RedirectToPage("./Index");
                }
                // Deserialize the JSON string to List of CartItems
                var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(SelectedProducts);
                // Redirect to success page or order confirmation
                
                if (cartItems == null || !cartItems.Any())
                {
                    return RedirectToPage("./Index");
                }
                var total = 0.0;
                var newOrder = await _apiClient.PostAsync("/orders/order?id=1", null);
                if(newOrder.ToString() == null) return RedirectToPage("./Index");
                var newOrderDe = JsonConvert.DeserializeObject<Order>(newOrder.Data.ToString());
                if(newOrder != null)
                foreach (var cartItem in cartItems)
                {
                    Console.WriteLine(cartItem.ProductId); ;
                    var product = await _apiClient.GetMinhAsync<ServiceResult>($"/products/{cartItem.ProductId}");
                    if (product != null && product.Status == 200 && product.Data != null)
                    {

                        if (product.Data!=null)
                        {
                            var responseData = (ServiceResult)product.Data;
                            var productDe = JsonConvert.DeserializeObject<Product>(responseData.Data.ToString());
                            var productCartResult = await _apiClient.PostMinhAsync($"/order-details?orderId={newOrderDe.OrderId}&quanity={cartItem.Quantity}", productDe);
                                if (productCartResult != null) await _apiClient.DeleteAsync($"/cart/product/{cartItem.ProductId}");
                        }

                    }
                }
                return RedirectToPage("");
            }
            catch (Exception ex)
            {
                // Handle any errors
                ModelState.AddModelError("", "An error occurred during checkout.");
                return Page();
            }
        }
    }
    public class IndextProductCartPage
    {
        public List<ProductCart>? ProductCart { get; set; }
    }
    public class CartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
