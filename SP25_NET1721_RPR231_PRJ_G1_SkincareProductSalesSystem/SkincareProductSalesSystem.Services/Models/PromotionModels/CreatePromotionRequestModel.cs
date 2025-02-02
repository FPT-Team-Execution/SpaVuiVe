using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Models.PromotionModels
{
	public class CreatePromotionRequestModel
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
