using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.SkinTestModels
{

    public class CreateSkinTestRequest
    {
        public string Question { get; set; } = string.Empty;

        public int? QuestionOrder { get; set; }

        public List<CreateSkinTestOptionRequest> Options { get; set; }
    }
    public class CreateSkinTestOptionRequest
    {
        public string OptionText { get; set; }

        public string SkinTypeId { get; set; }
    }
}
