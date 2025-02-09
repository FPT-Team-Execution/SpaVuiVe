using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class SkinTestRepository : GenericRepository<SkinTestQuestion>
    {
        public new async Task<SkinTestQuestion?> GetByIdAsync(string id)
        {
            return await _context.Set<SkinTestQuestion>().Include(q => q.SkinTestOptions).FirstOrDefaultAsync(x => x.QuestionId.Equals(id));
        }
        public new async Task<List<SkinTestQuestion>> GetAllAsync()
        {
            return await _context.Set<SkinTestQuestion>().Include(q => q.SkinTestOptions).ToListAsync();
        }
    }
}