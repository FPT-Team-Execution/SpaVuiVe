using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class CustomerProfile
{
    public string CustomerId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? SkinTypeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? LoyaltyPoints { get; set; }

    public string? PreferredPaymentMethod { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool? IsSubscribed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual SkinType? SkinType { get; set; }

    public virtual User? User { get; set; }
}
