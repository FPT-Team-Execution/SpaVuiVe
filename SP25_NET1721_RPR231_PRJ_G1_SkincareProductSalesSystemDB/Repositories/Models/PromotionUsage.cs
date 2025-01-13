using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class PromotionUsage
{
    public string UsageId { get; set; } = null!;

    public string? PromotionId { get; set; }

    public string? OrderId { get; set; }

    public decimal DiscountAmount { get; set; }

    public DateTime? UsedAt { get; set; }

    public bool? IsValid { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Promotion? Promotion { get; set; }
}
