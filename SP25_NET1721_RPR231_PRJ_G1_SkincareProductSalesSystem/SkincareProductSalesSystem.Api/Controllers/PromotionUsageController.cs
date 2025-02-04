using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Services;

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
				var result = await promotionUsageService.Create(request);
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
