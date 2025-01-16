using SkincareProductSalesSystem.Repositories.Models;
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
        Task<List<Brand>> GetAll();
        Task<Brand?> GetBrandById(string id);
    }
    public class BrandServices : IBrandService
    {
        private readonly BrandRepository _brandRepository;

        public BrandServices(BrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<List<Brand>> GetAll()
        {
            return await _brandRepository.GetAll();
        }

        public async Task<Brand?> GetBrandById(string id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }
    }
}
