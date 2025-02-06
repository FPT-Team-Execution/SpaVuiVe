using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.ProductPages;

public class Details : PageModel
{
    private ApiClient _apiClient;
    public DetailsProductPageData DetailsProductPageData { get; set; } = new();

    public Details(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }


    public async Task OnGetAsync()
    {
        var productId = Request.Query["productId"];
        var response = await _apiClient.GetAsync($"/products/{productId}");

        if (response.Status == 200 && response.Data is not null)
        {
            DetailsProductPageData.Product = JsonConvert.DeserializeObject<Product>(response.Data.ToString());
        }
    }
}

public class DetailsProductPageData
{
    public Product Product { get; set; } = new Product();
}