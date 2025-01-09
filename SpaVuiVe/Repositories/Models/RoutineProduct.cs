using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class RoutineProduct
{
    public string RoutineProductId { get; set; } = null!;

    public string? RoutineId { get; set; }

    public string? ProductId { get; set; }

    public string? Step { get; set; }

    public string? UsageInstructions { get; set; }

    public int? StepOrder { get; set; }

    public string? TimeOfDay { get; set; }

    public string? Frequency { get; set; }

    public bool? IsRequired { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual SkinCareRoutine? Routine { get; set; }
}
