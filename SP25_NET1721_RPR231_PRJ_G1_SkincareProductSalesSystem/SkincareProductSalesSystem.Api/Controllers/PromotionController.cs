using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareProductSalesSystem.Services.Models.PromotionModels;
using SkincareProductSalesSystem.Services.Services.Interfaces;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PromotionController : ControllerBase
	{
		private IPromotionService _promotionService;

		public PromotionController(IPromotionService promotionService)
		{
			_promotionService = promotionService;
		}

		[HttpPost("create")]
		[Authorize]
		public async Task<IActionResult> Create(CreatePromotionRequestModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var result = await _promotionService.Create(model);
				if (!result.IsSuccess)
				{
					return StatusCode(500, result.Message);
				}

				return Ok(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}		
		}

		[HttpGet("get")]
		[Authorize]
		public async Task<IActionResult> Get()
		{
			try
			{
				var result = await _promotionService.GetCodes();
				if (!result.IsSuccess)
				{
					return StatusCode(500, result.Message);
				};
				return Ok(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpDelete("delete")]
		public async Task<IActionResult> Delete(DeletePromotionRequestModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var result = await _promotionService.Delete(model);
				if (!result)
					return StatusCode(500);
				return Ok(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}
	}
}
