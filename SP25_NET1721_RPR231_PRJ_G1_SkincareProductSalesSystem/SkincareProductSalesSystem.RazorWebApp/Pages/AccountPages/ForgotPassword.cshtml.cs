using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
    public class ForgotPasswordModel : PageModel
    {
		[BindProperty]
		public ResetPasswordRequest ResetPasswordRequest { get; set; }

		private ApiClient _apiClient;

		public ForgotPasswordModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public string? ErrorMessage { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostForgotPassword()
		{
			var response = await _apiClient.PostAsync("/login", ResetPasswordRequest.Username);

			if (response.Status != 200)
			{
				ErrorMessage = response.Message;
				return Page();
			}

			return Page();
		}

    }

	public class ResetPasswordRequest 
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[RegularExpression(@"^.{5}$", ErrorMessage = "Mã bảo mật không hợp lệ")]
		public string Key { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ viết hoa, 1 chữ viết thường và 1 chữ số")]
		public string NewPassword { get; set; }
	}
}
