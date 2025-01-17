using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{

    public class CreateCategoryRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool? IsActive { get; set; }

        public string ParentCategoryId { get; set; }

        public int? DisplayOrder { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool? IsActive { get; set; }

        public string ParentCategoryId { get; set; }

        public int? DisplayOrder { get; set; }
    }

    public interface ICategoryService
    {
        Task<IServiceResult> GetAllAsync();
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(CreateCategoryRequest request);
        Task<IServiceResult> Update(string id, UpdateCategoryRequest request);
        Task<IServiceResult> Delete(string id);
    }

    public class CategoryService : ICategoryService
    {
        private UnitOfWork _unitOfWork;

        public CategoryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> Create(CreateCategoryRequest request)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive,
                ParentCategoryId = request.ParentCategoryId,
                DisplayOrder = request.DisplayOrder,
            };

            await _unitOfWork.CategoryRepository.CreateAsync(category);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = category,
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.CategoryRepository.RemoveAsync(category);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = category
            };
        }

        public async Task<IServiceResult> GetAllAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = categories
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var category = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (category == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = category
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateCategoryRequest request)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null) return new ServiceResult(404, "Không tìm thấy");

            category.Description = request.Description;
            category.Name = request.Name;
            category.ImageUrl = request.ImageUrl;
            category.ParentCategoryId = request.ParentCategoryId;
            category.DisplayOrder = request.DisplayOrder;

            await _unitOfWork.CategoryRepository.UpdateAsync(category);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = category
            };
        }
    }
}
