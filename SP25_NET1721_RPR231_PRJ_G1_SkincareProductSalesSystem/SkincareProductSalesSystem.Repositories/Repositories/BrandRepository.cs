
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Repositories.Repositories
{
    public class BrandRepository : GenericRepository<Brand>
    {
        public BrandRepository() { }
        public async Task<IEnumerable<Brand>> GetBrandsByName(int page, int size, string name)
        {
            return await _context.Brands.Where(b => b.Name.Contains(name)).Skip((page - 1) * size).Take(size).ToListAsync();
        }
    }
}