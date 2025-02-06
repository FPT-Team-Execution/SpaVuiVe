using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class SkinTestRepository : GenericRepository<SkinTestQuestion>
    {
        public new async Task<SkinTestQuestion?> GetByIdAsync(string id)
        {
            return await _context.Set<SkinTestQuestion>().FindAsync(id);
        }
      
    }
}