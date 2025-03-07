using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class PromotionRepository : GenericRepository<Promotion>
    {
		public async Task<Promotion?> GetByCodeAsync(string code)
		{
			return await _context.Promotions.FirstOrDefaultAsync(p => p.Code.Equals(code));
		}
    }
}