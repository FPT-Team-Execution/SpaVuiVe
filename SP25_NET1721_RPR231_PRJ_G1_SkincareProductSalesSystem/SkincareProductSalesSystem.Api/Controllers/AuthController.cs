using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;
using SkincareProductSalesSystem.Services.Base;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private IAuthService _authService;

		public AuthController(IAuthService userService)
		{
			_authService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid) 
				return BadRequest(ModelState);
			try
			{
				var result = await _authService.Register(model);
				return result != null ?
					StatusCode(result.Status, (result.Message.IsNullOrEmpty() ? result.Message : result.Data))
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequestModel model)
		{
			if (!ModelState.IsValid) 
				return BadRequest(ModelState);
			try
			{
				var result = await _authService.LoginByUsername(model);
				return result != null ?
					StatusCode(result.Status, result.Message.IsNullOrEmpty() ? result.Message : result.Data)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([Required] string username)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var result = await _authService.ForgotPassword(username);
				return result != null ?
					StatusCode(result.Status, (result.Message.IsNullOrEmpty() ? result.Message : result.Data))
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var result = await _authService.ResetPassword(model);
				return result != null ?
					StatusCode(result.Status, (result.Message.IsNullOrEmpty() ? result.Message : result.Data))
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}
	}
}
