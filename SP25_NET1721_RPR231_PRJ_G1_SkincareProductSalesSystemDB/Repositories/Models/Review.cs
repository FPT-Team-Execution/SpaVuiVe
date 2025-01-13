using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Review
{
    public string ReviewId { get; set; } = null!;

    public string? ProductId { get; set; }

    public string? CustomerId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsVerified { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public bool? IsVisible { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CustomerProfile? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
