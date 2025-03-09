using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class RoutineProductRepository : GenericRepository<RoutineProduct>
    {
        public RoutineProductRepository() { }

        public async Task<IEnumerable<RoutineProduct>> GetByRoutineIdAsync(string routineId)
        {
            return await _context.RoutineProducts.Where(rp => rp.RoutineId == routineId).ToListAsync();
        }
    }
}