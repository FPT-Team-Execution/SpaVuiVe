namespace SkincareProductSalesSystem.RazorWebApp.Models;

public class Category
{
    public string CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public string ParentCategoryId { get; set; }

    public int? DisplayOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}