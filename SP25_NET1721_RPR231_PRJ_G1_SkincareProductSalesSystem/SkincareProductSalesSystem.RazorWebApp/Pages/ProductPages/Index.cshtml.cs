using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.ProductPages;

public class Index : PageModel
{
    private readonly ApiClient _apiClient;
    public ContentData contentData { get; set; } = new();

    public Index(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task OnGetAsync()
    {
        var categoryTask = _apiClient.GetAsync("/categories?page=1&size=100");
        var productTask = _apiClient.GetAsync("/products?page=1&size=12");
        var brandTask = _apiClient.GetAsync("/brands?page=1&size=100");

        await Task.WhenAll(categoryTask, productTask, brandTask);

        var categoryResult = categoryTask.Result;
        var productResult = productTask.Result;
        var brandResult = brandTask.Result;

        contentData.Categories = GetItemsFromResponse<Category>(categoryResult).Items ??= new List<Category>();
        contentData.Products = GetItemsFromResponse<Product>(productResult);
        contentData.Brands = GetItemsFromResponse<Brand>(brandResult).Items ??= new List<Brand>();
    }

    private Paginate<T> GetItemsFromResponse<T>(ServiceResult response)
    {
        if (response.Status == 200 && response.Data != null)
        {
            var paginatedResult = JsonConvert.DeserializeObject<Paginate<T>>(response.Data.ToString());
            return paginatedResult;
        }

        return new Paginate<T>();
    }
}

public class ContentData
{
    public List<Category> Categories { get; set; } = new List<Category>();
    public Paginate<Product> Products { get; set; } = new Paginate<Product>();
    public List<Brand> Brands { get; set; } = new List<Brand>();
}