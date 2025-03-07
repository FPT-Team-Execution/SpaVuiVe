using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class PromotionUsageRepository : GenericRepository<PromotionUsage>
    {
		public async Task<int> GetUsageCountAsync(string promotionid)
		{
			return await _context.PromotionUsages.CountAsync(p => p.PromotionId.Equals(promotionid));
		}
    }
}