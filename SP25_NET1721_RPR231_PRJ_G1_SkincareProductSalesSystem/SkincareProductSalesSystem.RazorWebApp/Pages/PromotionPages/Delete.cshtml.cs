using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Protos.PromotionClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class DeleteModel : PageModel
    {
        private GrpcClient<PromotionServiceGRPC.PromotionServiceGRPCClient> _grpcClient;

		public DeleteModel(GrpcClient<PromotionServiceGRPC.PromotionServiceGRPCClient> grpcClient)
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

			var response = await _grpcClient.Client.GetAsync(new GetByIdRequestProto()
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
				return RedirectToPage("./Index");
			}

			var response = await _grpcClient.Client.DeleteAsync(new DeletePromotionRequestProto()
			{
				Id = id
			});

			return RedirectToPage("./Index");
		}
    }
}
