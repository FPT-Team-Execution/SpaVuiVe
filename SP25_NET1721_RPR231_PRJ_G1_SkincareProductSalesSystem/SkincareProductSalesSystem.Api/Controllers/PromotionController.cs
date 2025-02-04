using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;

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
				return result != null ? 
					StatusCode(result.Status, (result.Message.IsNullOrEmpty() ? result.Message: result.Data)) 
					: 
					StatusCode(500, "No Response");
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

		[HttpPut("update")]
		public async Task<IActionResult> Update(UpdatePromotionRequestModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var result = await _promotionService.Update(model);
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
