namespace SkincareProductSalesSystem.Repositories.Paginate;

public class Paginate<TResult>
{
    public int Size { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public List<TResult> Items { get; set; }
}