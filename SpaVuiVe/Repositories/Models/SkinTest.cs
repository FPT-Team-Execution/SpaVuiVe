using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class SkinTest
{
    public string TestId { get; set; } = null!;

    public string Question { get; set; } = null!;

    public string? OptionA { get; set; }

    public string? OptionB { get; set; }

    public string? OptionC { get; set; }

    public string? OptionD { get; set; }

    public string? CorrectSkinTypeId { get; set; }

    public int? QuestionOrder { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual SkinType? CorrectSkinType { get; set; }
}
