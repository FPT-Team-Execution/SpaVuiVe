using Azure;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Repositories.Paginate;
using SkincareProductSalesSystem.Repositories.Repositories;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface IBrandService
    {
        Task<IServiceResult> GetPaginate(int page, int size);
        Task<IServiceResult> GetBrandById(string id);
        Task<IServiceResult> GetBrandByName(int page, int size, string name);
        Task<IServiceResult> CreateBrand(Brand brand);
        Task<IServiceResult> UpdateBrand(Brand brand);
        Task<IServiceResult> DeleteBrand(string id);
    }
    public class BrandServices : IBrandService
    {
        private readonly BrandRepository _brandRepository;

        public BrandServices(BrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        //public async Task<IPaginate<Brand>> GetPaginate(int page, int size)
        //{
        //    return await _brandRepository.GetPagingListAsync(
        //            page: page,
        //            size: size
        //        );
        //}
        public async Task<IServiceResult> GetPaginate(int page, int size)
        {
            var response = await _brandRepository.GetPagingListAsync(
                    page: page,
                    size: size
                );
            return new ServiceResult
            {
                Status = 200,
                Message = "",
                Data = response
            };
        }
        //public async Task<Brand?> GetBrandById(string id)
        //{
        //    return await _brandRepository.GetByIdAsync(id);
        //}

        public async Task<IServiceResult> GetBrandById(string id)
        {
            var response = await _brandRepository.GetByIdAsync(id);
            return new ServiceResult
            {
                Status = 200,
                Message = "",
                Data = response
            };
        }

        //public async Task<IEnumerable<Brand>> GetBrandByName(int page, int size, string name)
        //{
        //    return await _brandRepository.GetBrandsByName(page, size, name);
        //}

        public async Task<IServiceResult> GetBrandByName(int page, int size, string name)
        {
            var response = await _brandRepository.GetBrandsByName(page, size, name);
            return new ServiceResult
            {
                Status = 200,
                Message = "",
                Data = response
            };
        }

        //public async Task<Brand?> CreateBrand(Brand brand)
        //{
        //    return (await _brandRepository.CreateAsync(brand) > 0)? brand : null;
        //}

        public async Task<IServiceResult> CreateBrand(Brand brand)
        {
            var response = await _brandRepository.CreateAsync(brand);
            return (response > 0) ? (new ServiceResult
            {
                Status = 200,
                Message = "Tạo thành công",
                Data = response
            }) :
            new ServiceResult
            {
                Status = 501,
                Message = "Tạo thất bại",
            };
        }

        //public async Task<Brand?> UpdateBrand(Brand brand)
        //{
        //    return (await _brandRepository.UpdateAsync(brand) > 0)? brand : null;
        //}
        public async Task<IServiceResult> UpdateBrand(Brand brand)
        {
            var response = await _brandRepository.UpdateAsync(brand);
            return (response > 0) ? (new ServiceResult
            {
                Status = 200,
                Message = "Cập nhật thành công",
                Data = response
            }) :
            new ServiceResult
            {
                Status = 501,
                Message = "Cập nhật thất bại",
            };
        }

        //public async Task<Brand?> DeleteBrand(string id)
        //{
        //    var brand = await _brandRepository.GetByIdAsync(id);
        //    return (await _brandRepository.RemoveAsync(brand))? brand : null;
        //}
        public async Task<IServiceResult> DeleteBrand(string id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            var reponse = await _brandRepository.RemoveAsync(brand);
            return (reponse) ? (new ServiceResult
            {
                Status = 200,
                Message = "Cập nhật thành công",
                Data = brand
            }) :
            new ServiceResult
            {
                Status = 501,
                Message = "Cập nhật thất bại",
            };
        }
    }
}
