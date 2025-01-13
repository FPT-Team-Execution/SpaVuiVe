using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public string? CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public string? ShippingAddress { get; set; }

    public decimal? ShippingFee { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CustomerProfile? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = new List<PromotionUsage>();
}
