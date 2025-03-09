using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Protos.AuthClient;
using SkincareProductSalesSystem.Common;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
    public class ForgotPasswordModel : PageModel
    {
		private ApiClient _apiClient;

		public ForgotPasswordModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
		[Required]
		public string Username { get; set; }
		public string? ErrorMessage { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				ErrorMessage = "Không thể gửi yêu cầu";
				return Page();
			}

			var response = await _apiClient.PostAsync("/forgot-password", Username);

			if (response.Status != 200)
			{
				ErrorMessage = response.Message;
				return Page();
			}
			HttpContext.Session.SetString("ForgotPasswordUsername", Username);
			return RedirectToPage("/AccountPages/ResetPassword");
		}
	}
}
