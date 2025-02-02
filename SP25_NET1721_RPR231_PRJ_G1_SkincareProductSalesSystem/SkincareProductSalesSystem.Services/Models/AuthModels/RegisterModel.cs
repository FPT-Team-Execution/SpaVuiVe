using SkincareProductSalesSystem.Repositories.Models;
using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.AuthModels
{
	public class RegisterModel : BaseModel
	{
		[Required]
		public string? Username { get; set; }
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? FullName { get; set; }
		[Required]
		[RegularExpression(@"^0\d{9}$", ErrorMessage = "Invalid Phone Number format")]
		public string? PhoneNumber { get; set; }
		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string Password { get; set; }
	}
}
