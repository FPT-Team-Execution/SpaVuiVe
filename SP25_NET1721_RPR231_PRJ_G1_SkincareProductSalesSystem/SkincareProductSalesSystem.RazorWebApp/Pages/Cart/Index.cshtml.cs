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
                            var productCartResult = await _apiClient.PutAsync($"/cart/product?quantity={quantity}", responseData.ProductInCart);
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
    }
    public class IndextProductCartPage
    {
        public List<ProductCart>? ProductCart { get; set; }
    }
}
