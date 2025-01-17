using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.AuthModels
{
	public class ResetPasswordRequestModel : BaseModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[RegularExpression(@"^.{5}$", ErrorMessage = "Incorrect Security Key")]
		public string Key { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string NewPassword { get; set; }

		

	}
}
