using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class SkinTypeRepository : GenericRepository<SkinType>
    {
       public new async Task<SkinType?> GetByIdAsync(string id)
        {
            return await _context.Set<SkinType>().FindAsync(id);
        }

    }
}