using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.SkinTestModels
{
    public class CreateSkinTestOptionRequest
    {
        public string OptionText { get; set; }

        public string SkinTypeId { get; set; }
    }
}
