using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionUsagePages
{
	public class CodeSubmitModel : PageModel
	{
		[BindProperty]
		public CreatePromotionUsageRequest Model { get; set; } = new();

		[BindProperty]
		public string? ResponseMessage {  get; set; }
		[BindProperty]
		public bool IsSuccess { get; set; }

		private ApiClient _apiClient;

		public CodeSubmitModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				var response = await _apiClient.PostAsync("api/promotionUsage/checkCode", Model);
				ResponseMessage = response.Message;
				IsSuccess = (response.Status >= 200 && response.Status <= 299);
				return Page();
			}
			catch (Exception ex)
			{
				return Page();
			}

		}
		public class CreatePromotionUsageRequest
		{
			[Required]
			public string PromoCode { get; set; }
			[Required]
			public string OrderId { get; set; }
			[Required]
			public string? Notes { get; set; }
		}
	}
}
