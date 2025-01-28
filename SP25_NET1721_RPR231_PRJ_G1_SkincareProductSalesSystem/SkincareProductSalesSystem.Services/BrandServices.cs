using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using SkincareProductSalesSystem.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IBrandService
    {
        Task<IPaginate<Brand>> GetPaginate(int page, int size);
        Task<Brand?> GetBrandById(string id);
        Task<IEnumerable<Brand>> GetBrandByName(int page, int size, string name);
        Task<Brand?> CreateBrand(Brand brand); 
        Task<Brand?> UpdateBrand(Brand brand);
        Task<Brand?> DeleteBrand(string id);
    }
    public class BrandServices : IBrandService
    {
        private readonly BrandRepository _brandRepository;

        public BrandServices(BrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IPaginate<Brand>> GetPaginate(int page, int size)
        {
            return await _brandRepository.GetPagingListAsync(
                    page: page,
                    size: size
                );
        }
        public async Task<Brand?> GetBrandById(string id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetBrandByName(int page, int size, string name)
        {
            return await _brandRepository.GetBrandsByName(page, size, name);
        }

        public async Task<Brand?> CreateBrand(Brand brand)
        {
            return (await _brandRepository.CreateAsync(brand) > 0)? brand : null;
        }

        public async Task<Brand?> UpdateBrand(Brand brand)
        {
            return (await _brandRepository.UpdateAsync(brand) > 0)? brand : null;
        }

        public async Task<Brand?> DeleteBrand(string id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return (await _brandRepository.RemoveAsync(brand))? brand : null;
        }
    }
}
