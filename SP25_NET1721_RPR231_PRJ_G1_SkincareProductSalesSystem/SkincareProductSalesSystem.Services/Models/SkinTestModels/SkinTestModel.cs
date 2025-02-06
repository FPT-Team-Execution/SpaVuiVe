using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.SkinTestModels
{
    public class SkinTestModel
    {
        public string QuestionId { get; set; }

        public string Question { get; set; }

        public int? QuestionOrder { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual List<SkinTestOptionModel> SkinTestOptions { get; set; } = new List<SkinTestOptionModel>();
    }

    public class SkinTestOptionModel
    {
        public string OptionId { get; set; }

        public string OptionText { get; set; }

        public string SkinTypeId { get; set; }
    }
}
