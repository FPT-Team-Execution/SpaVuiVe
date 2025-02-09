using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.SkinTestModels
{
    public class UpdateOptionRequest
    {
        public string OptionId { get; set; }
        public string OptionText { get; set; }
        public string SkinTypeId { get; set; }
    }
}
