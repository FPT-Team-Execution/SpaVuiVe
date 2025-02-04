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

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create(CreatePromotionRequest model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var response = await _promotionService.Create(model);
				return response != null ? 
					StatusCode(response.Status, (response)) 
					: 
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}		
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Get()
		{
			try
			{
				var response = await _promotionService.GetCodes();

				return response != null ?
					StatusCode(response.Status, (response))
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
		[HttpDelete]
		public async Task<IActionResult> Delete(DeletePromotionRequest model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var response = await _promotionService.Delete(model);
				return response != null ?
					StatusCode(response.Status, (response))
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update(UpdatePromotionRequest model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var response = await _promotionService.Update(model);
				return response != null ?
					StatusCode(response.Status, (response))
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
