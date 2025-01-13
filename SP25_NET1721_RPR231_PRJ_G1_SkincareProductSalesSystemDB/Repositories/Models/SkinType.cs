using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class SkinType
{
    public string SkinTypeId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Characteristics { get; set; }

    public string? RecommendedIngredients { get; set; }

    public string? AvoidIngredients { get; set; }

    public string? CareInstructions { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CustomerProfile> CustomerProfiles { get; set; } = new List<CustomerProfile>();

    public virtual ICollection<SkinCareRoutine> SkinCareRoutines { get; set; } = new List<SkinCareRoutine>();

    public virtual ICollection<SkinTest> SkinTests { get; set; } = new List<SkinTest>();
}
