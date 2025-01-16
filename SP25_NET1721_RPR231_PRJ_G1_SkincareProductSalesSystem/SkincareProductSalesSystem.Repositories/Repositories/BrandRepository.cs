using KoiMuseum.Data.Base;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories.Repositories
{
    public class BrandRepository : GenericRepository<Brand>
    {
        public async Task<List<Brand>> GetAll()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand?> GetBrandById(string id)
        {
            return await _context.Brands.FindAsync(id);
        }
    }
}