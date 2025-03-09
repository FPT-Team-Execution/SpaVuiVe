using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Protos.PromotionClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;
using static Grpc.Core.Metadata;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class CreateModel : PageModel
    {

		private ApiClient _apiClient;

		public CreateModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
        public CreatePromotionRequest Promotion { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _apiClient.PostAsync("/api/Promotion", Promotion);
			if (response.Status != 200)
			{
				Console.WriteLine(response.Message);
				return Page();
			}
            return RedirectToPage("./Index");
        }
    }

	public class CreatePromotionRequest
	{
		public string? Code { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public decimal DiscountAmount { get; set; }
		[Required]
		public decimal? MinimumPurchase { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime EndDate { get; set; }
		public int? UsageLimit { get; set; }
	}
}
