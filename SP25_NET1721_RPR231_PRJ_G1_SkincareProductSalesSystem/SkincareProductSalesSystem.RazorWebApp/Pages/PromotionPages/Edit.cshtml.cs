using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Protos.PromotionClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class EditModel : PageModel
    {
        private GrpcClient<PromotionServiceGRPC.PromotionServiceGRPCClient> _grpcClient;

		public EditModel(GrpcClient<PromotionServiceGRPC.PromotionServiceGRPCClient> grpcClient)
		{
			_grpcClient = grpcClient;
		}

		[BindProperty]
        public Promotion Promotion { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
				return RedirectToPage("./Index");
            }

            var response =  await _grpcClient.Client.GetAsync(new GetByIdRequestProto()
			{
				Id = id
			});

            if (response.Status != 200)
            {
				return RedirectToPage("./Index");
            }

            Promotion = new Promotion()
			{
				PromotionId = response.Data.PromotionId,
				Code = response.Data.Code,
				Name = response.Data.Name,
				CreatedAt = response.Data.CreatedAt.ToDateTime(),
				DiscountAmount = Convert.ToDecimal(response.Data.DiscountAmount),
				MinimumPurchase = Convert.ToDecimal(response.Data.MinimumPurchase),
				StartDate = response.Data.StartDate.ToDateTime(),
				EndDate = response.Data.EndDate.ToDateTime(),
				UsageLimit = response.Data.UsageLimit,
				IsActive = response.Data.IsActive
			};
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

			var response = await _grpcClient.Client.UpdateAsync(new UpdatePromotionRequestProto()
			{
				PromotionId = Promotion.PromotionId,
				Code = Promotion.Code ,
				Name = Promotion.Name,
				DiscountAmount = Convert.ToInt32(Promotion.DiscountAmount),
				MinimumPurchase= Convert.ToInt32(Promotion.MinimumPurchase),
				StartDate = Timestamp.FromDateTime(Promotion.StartDate.ToUniversalTime()),
				EndDate = Timestamp.FromDateTime(Promotion.EndDate.ToUniversalTime()),
				UsageLimit = Convert.ToInt32(Promotion.UsageLimit),
			});

			if (response.Status != 200)
			{
				return Page();
			}

            return RedirectToPage("./Index");
        }
    }
}
