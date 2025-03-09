using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Protos.PromotionClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class EditModel : PageModel
    {
		private ApiClient _apiClient;

		public EditModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
        public Promotion Promotion { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
				return RedirectToPage("./Index");
            }

            var response = await _apiClient.GetAsync($"/api/Promotion/{id}");

            if (response.Status != 200)
            {
				return RedirectToPage("./Index");
            }

			Promotion = JsonConvert.DeserializeObject<Promotion>(response.Data.ToString());
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			var response = await _apiClient.PutAsync("/api/Promotion", Promotion);

			//var response = await _grpcClient.Client.UpdateAsync(new UpdatePromotionRequestProto()
			//{
			//	PromotionId = Promotion.PromotionId,
			//	Code = Promotion.Code ,
			//	Name = Promotion.Name,
			//	DiscountAmount = Convert.ToInt32(Promotion.DiscountAmount),
			//	MinimumPurchase= Convert.ToInt32(Promotion.MinimumPurchase),
			//	StartDate = Timestamp.FromDateTime(Promotion.StartDate.ToUniversalTime()),
			//	EndDate = Timestamp.FromDateTime(Promotion.EndDate.ToUniversalTime()),
			//	UsageLimit = Convert.ToInt32(Promotion.UsageLimit),
			//});

			if (response.Status != 200)
			{
				return Page();
			}

            return RedirectToPage("./Index");
        }
    }
}
