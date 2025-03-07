using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Protos.AuthClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
	public class ResetPasswordModel : PageModel
	{

		private GrpcClient<AuthServiceGRPC.AuthServiceGRPCClient> _grpcClient;

		public ResetPasswordModel(GrpcClient<AuthServiceGRPC.AuthServiceGRPCClient> grpcClient)
		{
			_grpcClient = grpcClient;
		}

		[BindProperty]
		public ResetPasswordRequest ResetPasswordRequest { get; set; }

		public string ErrorMessage { get; set; }

		public void OnGet()
		{
			if (TempData["ForgotPasswordUsername"] == null)
				RedirectToPage("/AccountPages/ForgotPassword");
		}

		public async Task<IActionResult> OnPost()
		{

			if (HttpContext.Session.GetString("ForgotPasswordUsername") == null)
			{
				ErrorMessage = "Không tìm thấy tên tài khoản";
				return Page();
			}

			ResetPasswordRequest.Username = HttpContext.Session.GetString("ForgotPasswordUsername");
			if (!ModelState.IsValid)
			{
				ErrorMessage = ModelState.ToString();
				return Page();
			}
			var response = await _grpcClient.Client.ResetPasswordAsync(new ResetPasswordRequestProto()
			{
				Username = ResetPasswordRequest.Username,
				Password = ResetPasswordRequest.NewPassword,
				Key = ResetPasswordRequest.Key
			});

			if (response.Status != 200)
			{
				ErrorMessage = response.Message;
				return Page();
			}
			return RedirectToPage("/AccountPages/Login");
		}
	}

	public class ResetPasswordRequest
	{

		public string? Username { get; set; }

		[Required]
		[RegularExpression(@"^.{5}$", ErrorMessage = "Mã bảo mật không hợp lệ")]
		public string Key { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ viết hoa, 1 chữ viết thường và 1 chữ số")]
		public string NewPassword { get; set; }
	}
}
