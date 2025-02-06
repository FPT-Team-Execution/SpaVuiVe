using AutoMapper;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using SkincareProductSalesSystem.Services.Models.SkinTestModels;
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
        private readonly IMapper _mapper;

        public SkinTestService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<IServiceResult> Create(CreateSkinTestRequest request)
        {
            var newSkinTest = new SkinTestQuestion
            {
                QuestionId = Guid.NewGuid().ToString(),
                Question = request.Question,
                QuestionOrder = request.QuestionOrder,
                IsActive = true,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.SkinTestRepository.CreateAsync(newSkinTest);
            //create options
            if (request.Options != null && request.Options.Any())
            {
                foreach (var option in request.Options)
                {
                    var newOption = new SkinTestOption
                    {
                        OptionId = Guid.NewGuid().ToString(),
                        OptionText = option.OptionText,
                        QuestionId = newSkinTest.QuestionId,
                        SkinTypeId = option.SkinTypeId
                    };
                    await _unitOfWork.SkinTestOptionRepository.CreateAsync(newOption);

                }
            }
            await _unitOfWork.SkinTestOptionRepository.SaveAsync();

            await _unitOfWork.SkinTestRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = newSkinTest
            };
        }
        public async Task<IServiceResult> CreateOption(string questionId, CreateSkinTestOptionRequest request)
        {
            var question = await _unitOfWork.SkinTestRepository.GetByIdAsync(questionId);
            if (question == null) return new ServiceResult(404, "Không tìm thấy câu hỏi");
            var option = new SkinTestOption
            {
                OptionId = Guid.NewGuid().ToString(),
                OptionText = request.OptionText,
                QuestionId = question.QuestionId,
                SkinTypeId = request.SkinTypeId
            };


            await _unitOfWork.SkinTestOptionRepository.CreateAsync(option);

            await _unitOfWork.SkinTestOptionRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = option
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var skinTest = await _unitOfWork.SkinTestRepository.GetByIdAsync(id);
            if (skinTest == null) return new ServiceResult(404, "Không tìm thấy");
            //delete options
            var options = await _unitOfWork.SkinTestOptionRepository.GetByQuestionIdAsync(skinTest.QuestionId);
            foreach (var option in options)
            {
                await _unitOfWork.SkinTestOptionRepository.RemoveAsync(option);
            }

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

            SkinTestModel skinTestModel = _mapper.Map<SkinTestModel>(skinTest);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinTestModel
            };
        }

        public async Task<IServiceResult> GetAllAsync()
        {
            var skinTests = await _unitOfWork.SkinTestRepository.GetAllAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = _mapper.Map<List<SkinTestModel>>(skinTests)
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateSkinTestRequest skinTypeRequest)
        {
            var skinTest = await _unitOfWork.SkinTestRepository.GetByIdAsync(id);
            if (skinTest == null) return new ServiceResult(404, "Không tìm thấy");

            //update skin test
            skinTest.Question = skinTypeRequest.Question;
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
        public async Task<IServiceResult> UpdateOption(string questionId, UpdateOptionRequest request)
        {
            var question = await _unitOfWork.SkinTestRepository.GetByIdAsync(questionId);
            if (question == null) return new ServiceResult(404, "Không tìm thấy câu hỏi");

            var option = await _unitOfWork.SkinTestOptionRepository.GetByIdAsync(questionId, request.OptionId);
            if (option == null) return new ServiceResult(404, "Không tìm thấy lựa chọn");

            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(request.SkinTypeId);
            if (skinType == null) return new ServiceResult(404, "Không tìm thấy loại da");

            //update skin test option
            option.OptionText = request.OptionText;
            option.SkinTypeId = skinType.SkinTypeId;


            await _unitOfWork.SkinTestOptionRepository.UpdateAsync(option);
            await _unitOfWork.SkinTestOptionRepository.SaveAsync();
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = option
            };

        }

    }


}




