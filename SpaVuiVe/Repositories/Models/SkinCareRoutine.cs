using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class SkinCareRoutine
{
    public string RoutineId { get; set; } = null!;

    public string? SkinTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? MorningSteps { get; set; }

    public string? EveningSteps { get; set; }

    public string? WeeklySteps { get; set; }

    public string? Duration { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RoutineProduct> RoutineProducts { get; set; } = new List<RoutineProduct>();

    public virtual SkinType? SkinType { get; set; }
}
