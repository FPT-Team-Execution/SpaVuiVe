using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.SkinTestModels
{
    public class UpdateSkinTestRequest
    {
        public string Question { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        public int? QuestionOrder { get; set; }
    }
}
