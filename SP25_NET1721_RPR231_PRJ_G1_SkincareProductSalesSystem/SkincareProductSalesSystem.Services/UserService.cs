using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateUserRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public interface IUserService
    {
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> Create(CreateUserRequest request);
        Task<IServiceResult> Update(string id, UpdateUserRequest request);
        Task<IServiceResult> Delete(string id);
    }

    public class UserService : IUserService
    {
        private UnitOfWork _unitOfWork;

        public UserService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> Create(CreateUserRequest request)
        {
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Username = request.Username,
                PasswordHash = request.Password, // Nên hash mật khẩu trước khi lưu
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                RoleName = request.RoleName,
                IsActive = request.IsActive,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.UserRepository.CreateAsync(user);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = user
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.UserRepository.RemoveAsync(user);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = user
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            var users = await _unitOfWork.UserRepository.GetPagingListAsync(page: page, size: size);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = users
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = user
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateUserRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return new ServiceResult(404, "Không tìm thấy");

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.RoleName = request.RoleName;
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.Now;

            await _unitOfWork.UserRepository.UpdateAsync(user);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = user
            };
        }
    }
}
