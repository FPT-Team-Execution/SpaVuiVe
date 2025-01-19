using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SkincareProductSalesSystem.Services.Models.AuthModels;
using SkincareProductSalesSystem.Services.Services;
using SkincareProductSalesSystem.Services.Services.Interfaces;
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
				if (!result.IsSuccess)
					return StatusCode(500, result.Message);
				return StatusCode(200);
			}
			catch (Exception ex)
			{
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
				if (!result.IsSuccess)
					return StatusCode(500, result.Message);
				return StatusCode(200, result);
			}
			catch (Exception ex)
			{
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
				if (!result.IsSuccess)
					return StatusCode(500, result.Message);
				return StatusCode(200, result);
			}
			catch (Exception ex)
			{
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
				if (!result.IsSuccess)
					return StatusCode(500, result.Message);
				return StatusCode(200, result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


	}
}
