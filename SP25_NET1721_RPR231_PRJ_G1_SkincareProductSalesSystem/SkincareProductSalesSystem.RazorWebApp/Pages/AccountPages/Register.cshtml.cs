using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SkincareProductSalesSystem.Common;
using Protos.AuthClient;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
	public class RegisterModel : PageModel
	{
		private ApiClient _apiClient;

		public RegisterModel(ApiClient client)
		{
			_apiClient = client;
		}


		public string? ErrorMessage { get; set; }

		[BindProperty]
		public RegisterRequest RegisterRequest { get; set; }


		public void OnGet()
		{
		}

		[HttpPost]
		public async Task<IActionResult> OnPost()
		{
			try
			{
				if (!ModelState.IsValid)
				{
					ErrorMessage = "Invalid username or Password";
					return Page();
				}

				var response = await _apiClient.PostAsync("/register", RegisterRequest);
				if (response.Status != 200)
				{
					ErrorMessage = response.Message;
					return Page();
				}
				return RedirectToPage("/AccountPages/Login");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				ErrorMessage = ex.Message;
				return Page();
			};
		}
	}

	public class RegisterRequest
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string FullName { get; set; }
		[Required]
		[RegularExpression(@"^0\d{9}$", ErrorMessage = "Invalid Phone Number format")]
		public string PhoneNumber { get; set; }
		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string Password { get; set; }
	}
}
