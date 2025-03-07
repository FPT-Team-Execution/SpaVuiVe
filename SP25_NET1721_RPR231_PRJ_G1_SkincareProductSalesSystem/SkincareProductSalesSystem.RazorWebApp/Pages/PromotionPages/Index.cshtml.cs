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
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class IndexModel : PageModel
    {
		private GrpcClient<PromotionServiceGRPC.PromotionServiceGRPCClient> _grpcClient;

		public IndexModel(GrpcClient<PromotionServiceGRPC.PromotionServiceGRPCClient> grpcClient)
		{
			this._grpcClient = grpcClient;
		}

		public int PageNumber { get; set; } = 1;
		public int TotalPages { get; set; } = 1;
		public IList<Promotion> Promotion { get; set; } = new List<Promotion>();

        public async Task OnGetAsync()
        {
			var response = await _grpcClient.Client.GetAllAsync(new EmptyPromotionRequestProto());
			foreach (var item in response.Data) 
			{
				Promotion.Add(new Promotion()
				{
					PromotionId = item.PromotionId,
					Code = item.Code,
					Name = item.Name,
					CreatedAt = item.CreatedAt.ToDateTime(),
					DiscountAmount = Convert.ToDecimal(item.DiscountAmount),
					MinimumPurchase = Convert.ToDecimal(item.MinimumPurchase),
					StartDate = item.StartDate.ToDateTime(),
					EndDate = item.EndDate.ToDateTime(),
					UsageLimit = item.UsageLimit,
					IsActive = item.IsActive
				});
			}
        }
    }
}
