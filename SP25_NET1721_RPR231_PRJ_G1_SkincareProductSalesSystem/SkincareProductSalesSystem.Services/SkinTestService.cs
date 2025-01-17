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
    public interface ISkinTestService
    {
        Task<IServiceResult> GetAllAsync();
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(CreateSkinTestRequest request);
        Task<IServiceResult> Update(string id, UpdateSkinTestRequest skinType);
        Task<IServiceResult> Delete(string id);

    }
    public class SkinTestService : ISkinTestService
    {
        private readonly UnitOfWork _unitOfWork;

        public SkinTestService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IServiceResult> Create(CreateSkinTestRequest request)
        {
            var newSkinTest = new SkinTest
            {
                TestId = Guid.NewGuid().ToString(),
                OptionA = request.OptionA,
                OptionB = request.OptionB,
                OptionC = request.OptionC,
                OptionD = request.OptionD,
                Question = request.Question,
                CorrectSkinTypeId = request.CorrectSkinTypeId,
                QuestionOrder = request.QuestionOrder,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.SkinTestRepository.CreateAsync(newSkinTest);
            await _unitOfWork.SkinTestRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = newSkinTest
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var skinTest = await _unitOfWork.SkinTestRepository.GetByIdAsync(id);
            if (skinTest == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.SkinTestRepository.RemoveAsync(skinTest);
            await _unitOfWork.SkinTestRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinTest
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var skinTest = await _unitOfWork.SkinTestRepository.GetByIdAsync(id);
            if (skinTest == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinTest
            };
        }

        public async Task<IServiceResult> GetAllAsync()
        {
            var skinTypes = await _unitOfWork.SkinTestRepository.GetAllAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinTypes
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateSkinTestRequest skinTypeRequest)
        {
            var skinTest = await _unitOfWork.SkinTestRepository.GetByIdAsync(id);
            if (skinTest == null) return new ServiceResult(404, "Không tìm thấy");

            //update skin type
            skinTest.OptionA = skinTypeRequest.OptionA;
            skinTest.OptionB = skinTypeRequest.OptionB;
            skinTest.OptionC = skinTypeRequest.OptionC;
            skinTest.OptionD = skinTypeRequest.OptionD;
            skinTest.Question = skinTypeRequest.Question;
            skinTest.CorrectSkinTypeId = skinTypeRequest.CorrectSkinTypeId;
            skinTest.QuestionOrder = skinTypeRequest.QuestionOrder;


            await _unitOfWork.SkinTestRepository.UpdateAsync(skinTest);
            await _unitOfWork.SkinTestRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinTest
            };

        }

    }


}

public class UpdateSkinTestRequest
{
    public string Question { get; set; } = string.Empty;

    public string OptionA { get; set; } = string.Empty;

    public string OptionB { get; set; } = string.Empty;

    public string OptionC { get; set; } = string.Empty;

    public string OptionD { get; set; } = string.Empty;

    public string CorrectSkinTypeId { get; set; } = string.Empty;

    public int? QuestionOrder { get; set; }
}

public class CreateSkinTestRequest
{
    public string Question { get; set; } = string.Empty;

    public string OptionA { get; set; } = string.Empty;

    public string OptionB { get; set; } = string.Empty;

    public string OptionC { get; set; } = string.Empty;

    public string OptionD { get; set; } = string.Empty;

    public string CorrectSkinTypeId { get; set; } = string.Empty;

    public int? QuestionOrder { get; set; }
}

