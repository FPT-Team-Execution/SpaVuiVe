using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class SkinTestRepository : GenericRepository<SkinTest>
    {
        public new async Task<SkinTest?> GetByIdAsync(string id)
        {
            return await _context.Set<SkinTest>().FindAsync(id);
        }
    }
}