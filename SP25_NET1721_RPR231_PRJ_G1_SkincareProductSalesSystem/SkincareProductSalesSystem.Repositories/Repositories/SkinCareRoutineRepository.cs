using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories
{
    public class SkinCareRoutineRepository : GenericRepository<SkinCareRoutine>
    {
        public SkinCareRoutineRepository() { }

        public async Task<IEnumerable<SkinCareRoutine>> GetBySkinCareRoutineById(string id)
        {
            return await _context.SkinCareRoutines.Include(s => s.RoutineProducts).Where(x => x.RoutineId == id).ToListAsync();
        }
    }
}