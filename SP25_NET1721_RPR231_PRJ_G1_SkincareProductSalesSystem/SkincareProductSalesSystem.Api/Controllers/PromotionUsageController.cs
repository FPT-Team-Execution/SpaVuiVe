using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PromotionUsageController : ControllerBase
	{
		private IPromotionUsageService promotionUsageService;

		public PromotionUsageController(IPromotionUsageService promotionUsageService)
		{
			this.promotionUsageService = promotionUsageService;
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreatePromotionUsageRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await promotionUsageService.Create(request);
				return response != null ?
					StatusCode(response.Status, response)
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
		public async Task<IActionResult> Update(UpdatePromotionUsageRequest request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await promotionUsageService.Update(request);
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody][Required] string promoUsageId)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var response = await promotionUsageService.Delete(promoUsageId);
				return response != null ?
					StatusCode(response.Status, response)
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
		public async Task<IActionResult> Get()
		{
			try
			{
				var response = await promotionUsageService.GetAll();
				return response != null ?
					StatusCode(response.Status, response)
					:
					StatusCode(500, "No Response");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			try
			{
				var response = await promotionUsageService.GetById(id);
				return response != null ?
					StatusCode(response.Status, response)
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
