using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Protos;
using Protos.SkinTypesClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.ProductPages;

public class Index : PageModel
{
    private readonly ApiClient _apiClient;
    private readonly GrpcClient<ProductGrpcProto.ProductGrpcProtoClient> _grpcClient;

    public IndexProductPageData indexProductPageData { get; set; } = new();

    public int Page { get; set; }
    public int Size { get; set; }
    public string? Category { get; set; }
    public string? FilterBy { get; set; }
    public string? FilterQuery { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }

    public Index(ApiClient apiClient, GrpcClient<ProductGrpcProto.ProductGrpcProtoClient> grpcClient)
    {
        _apiClient = apiClient;
        _grpcClient = grpcClient;
    }

    //public async Task OnGetAsync()
    //{
    //    Page = int.TryParse(Request.Query["page"], out var page) ? page : 1;

    //    Size = int.TryParse(Request.Query["size"], out var size) ? size : 10;

    //    FilterBy = "Name";

    //    FilterQuery = Request.Query["filterQuery"];

    //    Category = Request.Query["category"];

    //    SortBy = "Price";

    //    IsAsc = bool.TryParse(Request.Query["isAsc"], out var isAsc) ? isAsc : true;

    //    var categoryTask = _apiClient.GetAsync("/categories?page=1&size=100");
    //    var productTask = _apiClient.GetAsync(
    //        $"/products?FilterBy={FilterBy}&FilterQuery={FilterQuery}&Page={Page}&Size={Size}&Category={Category}&SortBy={SortBy}&IsAsc={IsAsc}"
    //    );
    //    var brandTask = _apiClient.GetAsync("/brands?page=1&size=100");

    //    await Task.WhenAll(categoryTask, productTask, brandTask);

    //    var categoryResult = categoryTask.Result;
    //    var productResult = productTask.Result;
    //    var brandResult = brandTask.Result;

    //    indexProductPageData.Categories = GetItemsFromResponse<Models.Category>(categoryResult).Items ?? new List<Models.Category>();
    //    indexProductPageData.Products = GetItemsFromResponse<Product>(productResult);
    //    indexProductPageData.Brands = GetItemsFromResponse<Brand>(brandResult).Items ?? new List<Brand>();
    //}

    public async Task OnGetAsync()
    {
        Page = int.TryParse(Request.Query["page"], out var page) ? page : 1;

        Size = int.TryParse(Request.Query["size"], out var size) ? size : 10;

        FilterBy = "Name";

        FilterQuery = Request.Query["filterQuery"];

        Category = Request.Query["category"];

        SortBy = "Price";

        IsAsc = bool.TryParse(Request.Query["isAsc"], out var isAsc) ? isAsc : true;

        var categoryTask = _apiClient.GetAsync("/categories?page=1&size=100");
        var brandTask = _apiClient.GetAsync("/brands?page=1&size=100");

        var productGrpc = await _grpcClient.Client.GetAllProductsAsync(new GetAllProductQueryProto
        {
            Category = Category,
            FilterBy = FilterBy,
            FilterQuery = FilterQuery,
            IsAsc = IsAsc,
            Page = Page,
            Size = Size,
            SortBy = SortBy,
        });

        await Task.WhenAll(categoryTask, brandTask);

        var categoryResult = categoryTask.Result;
        var productResult = productGrpc.Data;
        var brandResult = brandTask.Result;



        indexProductPageData.Categories = GetItemsFromResponse<Models.Category>(categoryResult).Items ?? new List<Models.Category>();

        List<Product> items = new List<Product>();
        foreach (var item in productResult.Items)
        {
            items.Add(new Product
            {
                ProductId = item.ProductId,
                BrandId = item.BrandId,
                Name = item.Name,
                CategoryId = item.CategoryId,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Ingredients = item.Ingredients,
                Price = (decimal)item.Price,
                StockQuantity = item.StockQuantity,
            });
        }
        indexProductPageData.Products = new Paginate<Product>()
        {
            Page = productResult.Page,
            Size = productResult.Size,
            Total = productResult.Total,
            TotalPages = productResult.TotalPages,
            Items = items,

        };

        indexProductPageData.Brands = GetItemsFromResponse<Brand>(brandResult).Items ?? new List<Brand>();
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

public class IndexProductPageData
{
    public List<Models.Category> Categories { get; set; } = new List<Models.Category>();
    public Paginate<Product> Products { get; set; } = new Paginate<Product>();
    public List<Brand> Brands { get; set; } = new List<Brand>();
}