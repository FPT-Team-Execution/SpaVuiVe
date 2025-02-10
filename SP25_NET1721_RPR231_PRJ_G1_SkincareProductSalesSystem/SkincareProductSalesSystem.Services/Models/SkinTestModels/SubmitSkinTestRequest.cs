using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.SkinTestModels
{
    public class SubmitSkinTestRequest
    {
        public Dictionary<string, SkinTestOption> ChosenOptions { get; set; }
       
    }
}
