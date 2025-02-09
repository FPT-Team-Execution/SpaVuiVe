using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.AuthModels
{
	public class ForgotPasswordResponseModel : BaseModel
	{
		public string? Key { get; set; }
	}
}
