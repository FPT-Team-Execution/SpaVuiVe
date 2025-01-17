using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services.Models.AuthModels;
using SkincareProductSalesSystem.Services.Services;
using SkincareProductSalesSystem.Services.Services.Interfaces;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private IAuthService _userService;

		public AuthController(IAuthService userService)
		{
			_userService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid) 
				return BadRequest(ModelState);
			try
			{
				var result = await _userService.Register(model);
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
				var result = await _userService.LoginByUsername(model);
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
