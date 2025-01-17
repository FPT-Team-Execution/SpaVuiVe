using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public interface ISkinTypeService
    {
        Task<IServiceResult> GetAllAsync();
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(CreateSkinTypeRequest request);
        Task<IServiceResult> Update(string id, UpdateSkinTypeRequest skinType);
        Task<IServiceResult> Delete(string id);

    }
    public class SkinTypeService : ISkinTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        public SkinTypeService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IServiceResult> Create(CreateSkinTypeRequest request)
        {
            var newSkinType = new SkinType
            {
                SkinTypeId = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Characteristics = request.Characteristics,
                AvoidIngredients = request.AvoidIngredients,
                CareInstructions = request.CareInstructions,
                IsActive = false, //default is not active
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.SkinTypeRepository.CreateAsync(newSkinType);
            await _unitOfWork.SkinTypeRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = newSkinType
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(id);
            if (skinType == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.SkinTypeRepository.RemoveAsync(skinType);
            await _unitOfWork.SkinTypeRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinType
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(id);
            if (skinType == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinType
            };
        }

        public async Task<IServiceResult> GetAllAsync()
        {
            var skinTypes = await _unitOfWork.SkinTypeRepository.GetAllAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinTypes
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateSkinTypeRequest skinTypeRequest)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(id);
            if (skinType == null) return new ServiceResult(404, "Không tìm thấy");

            //update skin type
            skinType.Name = skinTypeRequest.Name;
            skinType.Description = skinTypeRequest.Description;
            skinType.Characteristics = skinTypeRequest.Characteristics;
            skinType.AvoidIngredients = skinTypeRequest.AvoidIngredients;
            skinType.CareInstructions = skinTypeRequest.CareInstructions;
            //* update update time
            skinType.UpdatedAt = DateTime.Now;


            await _unitOfWork.SkinTypeRepository.UpdateAsync(skinType);
            await _unitOfWork.SkinTypeRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinType
            };

        }

    }


}

public class UpdateSkinTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Characteristics { get; set; } = string.Empty;
    public string AvoidIngredients { get; set; } = string.Empty;
    public string CareInstructions { get; set; } = string.Empty;
}

public class CreateSkinTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Characteristics { get; set; } = string.Empty;
    public string AvoidIngredients { get; set; } = string.Empty;
    public string CareInstructions { get; set; } = string.Empty;
}
