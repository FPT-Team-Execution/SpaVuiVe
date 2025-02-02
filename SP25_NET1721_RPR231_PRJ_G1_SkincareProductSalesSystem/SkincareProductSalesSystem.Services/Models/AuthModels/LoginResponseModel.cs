using Microsoft.IdentityModel.Tokens;
using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.AuthModels
{
	public class LoginResponseModel : BaseModel
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }
	}
}
