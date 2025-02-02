using SkincareProductSalesSystem.Repositories.Models;
using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.PromotionModels
{
	public class GetPromotionResponseModel : BaseModel
	{
		public List<Promotion> promotions { get; set; }
	}
}
