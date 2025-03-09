using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public class SkinCareRoutineRequest
    {
        public string SkinTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MorningSteps { get; set; }
        public string EveningSteps { get; set; }
        public string WeeklySteps { get; set; }
        public string Duration { get; set; }
        public bool? IsActive { get; set; }
    }

    public interface ISkinCareRoutineService
    {
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(SkinCareRoutineRequest request);
        Task<IServiceResult> Update(string id, SkinCareRoutineRequest request);
        Task<IServiceResult> Delete(string id);
    }

    public class SkinCareRoutineService : ISkinCareRoutineService
    {
        private readonly UnitOfWork _unitOfWork;

        public SkinCareRoutineService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IServiceResult> Create(SkinCareRoutineRequest request)
        {
            // Validate request
            if (request == null)
                return new ServiceResult(400, "Yêu cầu không hợp lệ");

            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(request.SkinTypeId);
            if (skinType == null)
                return new ServiceResult(404, "Loại da không tồn tại");

            // Map request to SkinCareRoutine entity
            var skinCareRoutine = new SkinCareRoutine
            {
                RoutineId = Guid.NewGuid().ToString(),
                SkinTypeId = request.SkinTypeId,
                Name = request.Name,
                Description = request.Description,
                MorningSteps = request.MorningSteps,
                EveningSteps = request.EveningSteps,
                WeeklySteps = request.WeeklySteps,
                Duration = request.Duration,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };

            // Add to database
            await _unitOfWork.SkinCareRoutineRepository.CreateAsync(skinCareRoutine);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinCareRoutine
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            // Find the SkinCareRoutine by ID
            var skinCareRoutine = await _unitOfWork.SkinCareRoutineRepository.GetByIdAsync(id);
            if (skinCareRoutine == null)
                return new ServiceResult(404, "Không tìm thấy");

            // Delete from database
            await _unitOfWork.SkinCareRoutineRepository.RemoveAsync(skinCareRoutine);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinCareRoutine
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            // Get paginated list of SkinCareRoutines
            var skinCareRoutines = await _unitOfWork.SkinCareRoutineRepository.GetPagingListAsync(page: page, size: size);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinCareRoutines
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            // Find the SkinCareRoutine by ID
            var skinCareRoutine = await _unitOfWork.SkinCareRoutineRepository.GetBySkinCareRoutineById(id);
            if (skinCareRoutine == null)
                return new ServiceResult(404, "Không tìm thấy");

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = skinCareRoutine
            };
        }

        public async Task<IServiceResult> Update(string id, SkinCareRoutineRequest request)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(request.SkinTypeId);
            if (skinType == null)
                return new ServiceResult(404, "Loại da không tồn tại");
            // Find the existing SkinCareRoutine by ID
            var existingSkinCareRoutine = await _unitOfWork.SkinCareRoutineRepository.GetByIdAsync(id);
            if (existingSkinCareRoutine == null)
                return new ServiceResult(404, "Không tìm thấy");

            // Update properties
            existingSkinCareRoutine.SkinTypeId = request.SkinTypeId;
            existingSkinCareRoutine.Name = request.Name;
            existingSkinCareRoutine.Description = request.Description;
            existingSkinCareRoutine.MorningSteps = request.MorningSteps;
            existingSkinCareRoutine.EveningSteps = request.EveningSteps;
            existingSkinCareRoutine.WeeklySteps = request.WeeklySteps;
            existingSkinCareRoutine.Duration = request.Duration;
            existingSkinCareRoutine.IsActive = request.IsActive;
            existingSkinCareRoutine.UpdatedAt = DateTime.UtcNow;

            // Save changes to database
            await _unitOfWork.SkinCareRoutineRepository.UpdateAsync(existingSkinCareRoutine);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = existingSkinCareRoutine
            };
        }
    }
}