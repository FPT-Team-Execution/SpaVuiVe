using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public class RoutineProductRequest
    {
        public string RoutineId { get; set; }

        public string ProductId { get; set; }

        public string Step { get; set; }

        public string UsageInstructions { get; set; }

        public int? StepOrder { get; set; }

        public string TimeOfDay { get; set; }

        public string Frequency { get; set; }

        public bool? IsRequired { get; set; }
    }

    public interface IRoutineProductService
    {
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(RoutineProductRequest request);
        Task<IServiceResult> Update(string id, RoutineProductRequest request);
        Task<IServiceResult> Delete(string id);
        Task<IServiceResult> GetByRoutineId(string routineId);
    }

    public class RoutineProductService : IRoutineProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public RoutineProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IServiceResult> Create(RoutineProductRequest request)
        {
            // Validate request
            if (request == null)
                return new ServiceResult(400, "Yêu cầu không hợp lệ");

            // Validate TimeOfDay
            if (request.TimeOfDay != "Morning" && request.TimeOfDay != "Evening" && request.TimeOfDay != "Both")
                return new ServiceResult(400, "Thời gian trong ngày không hợp lệ. Nó phải là Morning, Evening, hoặc Both.");

            // Validate StepOrder
            if (request.StepOrder <= 0)
                return new ServiceResult(400, "Số bước phải lớn hơn 0.");

            // Validate Frequency
            if (request.Frequency != "Daily" && request.Frequency != "Weekly")
                return new ServiceResult(400, "Tần suất không hợp lệ. Nó phải là Daily hoặc Weekly.");

            var routine = await _unitOfWork.SkinCareRoutineRepository.GetByIdAsync(request.RoutineId);
            if (routine == null)
                return new ServiceResult(404, "Không tìm thấy chăm sóc da.");

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return new ServiceResult(404, "Không tìm thấy sản phẩm.");

            RoutineProduct routineProduct = new RoutineProduct
            {
                RoutineId = request.RoutineId,
                ProductId = request.ProductId,
                Step = request.Step,
                UsageInstructions = request.UsageInstructions,
                StepOrder = request.StepOrder,
                TimeOfDay = request.TimeOfDay,
                Frequency = request.Frequency,
                IsRequired = request.IsRequired
            };

            routineProduct.RoutineProductId = Guid.NewGuid().ToString();

            // Add to database
            await _unitOfWork.RoutineProductRepository.CreateAsync(routineProduct);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = request
            };
        }


        public async Task<IServiceResult> Delete(string id)
        {
            // Find the RoutineProduct by ID
            var routineProduct = await _unitOfWork.RoutineProductRepository.GetByIdAsync(id);
            if (routineProduct == null)
                return new ServiceResult(404, "Không tìm thấy");

            // Delete from database
            await _unitOfWork.RoutineProductRepository.RemoveAsync(routineProduct);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = routineProduct
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            // Get paginated list of RoutineProducts
            var routineProducts = await _unitOfWork.RoutineProductRepository.GetPagingListAsync(page: page, size: size);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = routineProducts
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            // Find the RoutineProduct by ID
            var routineProduct = await _unitOfWork.RoutineProductRepository.GetByIdAsync(id);
            if (routineProduct == null)
                return new ServiceResult(404, "Không tìm thấy");

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = routineProduct
            };
        }

        public async Task<IServiceResult> GetByRoutineId(string routineId)
        {
            if (string.IsNullOrEmpty(routineId))
                return new ServiceResult(400, "Routine ID không hợp lệ.");

            // Get paginated list of RoutineProducts by RoutineId
            var routineProducts = await _unitOfWork.RoutineProductRepository.GetByRoutineIdAsync(routineId);

            if (routineProducts == null || !routineProducts.Any())
                return new ServiceResult(404, "Không tìm thấy sản phẩm cho chăm sóc da này.");

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = routineProducts
            };
        }


        public async Task<IServiceResult> Update(string id, RoutineProductRequest request)
        {
            // Validate request
            if (request == null)
                return new ServiceResult(400, "Yêu cầu không hợp lệ");

            // Validate TimeOfDay
            if (request.TimeOfDay != "Morning" && request.TimeOfDay != "Evening" && request.TimeOfDay != "Both")
                return new ServiceResult(400, "Thời gian trong ngày không hợp lệ. Nó phải là Morning, Evening, hoặc Both.");

            // Validate StepOrder
            if (request.StepOrder <= 0)
                return new ServiceResult(400, "Số bước phải lớn hơn 0.");

            // Validate Frequency
            if (request.Frequency != "Daily" && request.Frequency != "Weekly")
                return new ServiceResult(400, "Tần suất không hợp lệ. Nó phải là Daily hoặc Weekly.");

            var routine = await _unitOfWork.SkinCareRoutineRepository.GetByIdAsync(request.RoutineId);
            if (routine == null)
                return new ServiceResult(404, "Không tìm thấy chăm sóc da.");

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return new ServiceResult(404, "Không tìm thấy sản phẩm.");
            // Find the existing RoutineProduct by ID
            var existingRoutineProduct = await _unitOfWork.RoutineProductRepository.GetByIdAsync(id);
            if (existingRoutineProduct == null)
                return new ServiceResult(404, "Không tìm thấy");

            // Update properties
            existingRoutineProduct.RoutineId = request.RoutineId;
            existingRoutineProduct.ProductId = request.ProductId;
            existingRoutineProduct.Step = request.Step;
            existingRoutineProduct.UsageInstructions = request.UsageInstructions;
            existingRoutineProduct.StepOrder = request.StepOrder;
            existingRoutineProduct.TimeOfDay = request.TimeOfDay;
            existingRoutineProduct.Frequency = request.Frequency;
            existingRoutineProduct.IsRequired = request.IsRequired;

            // Save changes to database
            await _unitOfWork.RoutineProductRepository.UpdateAsync(existingRoutineProduct);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = existingRoutineProduct
            };
        }
    }
}