using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.PromotionModels
{
	public class UpdatePromotionRequestModel
	{
		[Required]
		public string PromotionId { get; set; }
		public string? Code { get; set; }	
		public string? Name { get; set; }
		public decimal? DiscountAmount { get; set; }
		public decimal? MinimumPurchase { get; set; }	
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? UsageLimit { get; set; }
	}
}
