using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Protos.PromotionClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Paginate;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
	public class IndexModel : PageModel
	{

		private ApiClient _apiClient;

		public IndexModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public int PageNumber { get; set; } = 1;
		public int TotalPages { get; set; } = 1;
		public string Errors { get; set; }
		public IList<Promotion> Promotion { get; set; } = new List<Promotion>();

		public async Task<IActionResult> OnGetAsync()
		{
			var response = await _apiClient.GetMinhAsync<Paginate<Promotion>>($"/api/Promotion/{PageNumber}/10");

			if (response.Data == null)
			{
				return Page();
			}

			var responseData = (Paginate<Promotion>)response.Data;
			PageNumber = responseData.Page;
			TotalPages = responseData.TotalPages;
			foreach (var item in responseData.Items)
			{
				Promotion.Add(new Promotion()
				{
					PromotionId = item.PromotionId,
					Code = item.Code,
					Name = item.Name,
					CreatedAt = item.CreatedAt,
					DiscountAmount = Convert.ToDecimal(item.DiscountAmount),
					MinimumPurchase = Convert.ToDecimal(item.MinimumPurchase),
					StartDate = item.StartDate,
					EndDate = item.EndDate,
					UsageLimit = item.UsageLimit,
					IsActive = item.IsActive
				});
			}
			return Page();
		}
	}
}
