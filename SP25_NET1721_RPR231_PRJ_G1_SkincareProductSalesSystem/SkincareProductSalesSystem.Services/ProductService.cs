using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SkincareProductSalesSystem.Services
{
    public class CreateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public string BrandId { get; set; }

        public int? StockQuantity { get; set; }
        
        public IFormFile ImageFile { get; set; }

        public string Ingredients { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UpdateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public string BrandId { get; set; }

        public int? StockQuantity { get; set; }
        
        public IFormFile ImageFile { get; set; }

        public string Ingredients { get; set; }

        public bool? IsActive { get; set; }
    }

    public interface IProductService
    {
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(CreateProductRequest request);
        Task<IServiceResult> Update(string id, UpdateProductRequest request);
        Task<IServiceResult> Delete(string id);
    }

    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> Create(CreateProductRequest request)
        {
            var product = new Product
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = request.Name,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                Price = request.Price,
                ImageUrl = "",
                Description = request.Description,
                IsActive = request.IsActive,
                StockQuantity = request.StockQuantity,
                Ingredients = request.Ingredients,
            };

            var result = await _unitOfWork.ProductRepository.CreateAsync(product);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.ProductRepository.RemoveAsync(product);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            var products = await _unitOfWork.SkinTestRepository.GetPagingListAsync(page: page, size: size);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = products
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return new ServiceResult(404, "Không tìm thấy");

            //update skin type
            product.Name = request.Name;
            product.Description = request.Description;
            product.CategoryId = request.CategoryId;
            product.BrandId = request.BrandId;
            product.Price = request.Price;
            product.ImageUrl = "";
            product.Ingredients = request.Ingredients;
            product.StockQuantity = request.StockQuantity;


            await _unitOfWork.ProductRepository.UpdateAsync(product);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = product
            };
        }
    }
}