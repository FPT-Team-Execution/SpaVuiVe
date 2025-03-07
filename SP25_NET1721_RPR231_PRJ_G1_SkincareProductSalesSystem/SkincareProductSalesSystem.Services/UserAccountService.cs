using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public class CreateUserAccountRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeCode { get; set; }
        public int RoleId { get; set; }
        public string RequestCode { get; set; }
        public string ApplicationCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateUserAccountRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeCode { get; set; }
        public int RoleId { get; set; }
        public string ApplicationCode { get; set; }
        public bool IsActive { get; set; }
    }

    public interface IUserAccountService
    {
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(int id);
        Task<IServiceResult> Create(CreateUserAccountRequest request);
        Task<IServiceResult> Update(int id, UpdateUserAccountRequest request);
        Task<IServiceResult> Delete(int id);
    }

    public class UserAccountService : IUserAccountService
    {
        private UnitOfWork _unitOfWork;

        public UserAccountService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> Create(CreateUserAccountRequest request)
        {
            var userAccount = new UserAccount
            {
                UserName = request.UserName,
                Password = request.Password,
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                EmployeeCode = request.EmployeeCode,
                RoleId = request.RoleId,
                RequestCode = request.RequestCode,
                ApplicationCode = request.ApplicationCode,
                IsActive = request.IsActive,
                CreatedDate = DateTime.Now,
                CreatedBy = "System"
            };

            await _unitOfWork.UserAccountRepository.CreateAsync(userAccount);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccount,
            };
        }

        public async Task<IServiceResult> Delete(int id)
        {
            var userAccount = await _unitOfWork.UserAccountRepository.GetByIdAsync(id);
            if (userAccount == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.UserAccountRepository.RemoveAsync(userAccount);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccount
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            var userAccounts = await _unitOfWork.UserAccountRepository.GetPagingListAsync(page: page, size: size);
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccounts
            };
        }

        public async Task<IServiceResult> GetAsync(int id)
        {
            var userAccount = await _unitOfWork.UserAccountRepository.GetByIdAsync(id);
            if (userAccount == null) return new ServiceResult(404, "Không tìm thấy");
            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccount
            };
        }

        public async Task<IServiceResult> Update(int id, UpdateUserAccountRequest request)
        {
            var userAccount = await _unitOfWork.UserAccountRepository.GetByIdAsync(id);

            if (userAccount == null) return new ServiceResult(404, "Không tìm thấy");

            userAccount.FullName = request.FullName;
            userAccount.Email = request.Email;
            userAccount.Phone = request.Phone;
            userAccount.EmployeeCode = request.EmployeeCode;
            userAccount.RoleId = request.RoleId;
            userAccount.ApplicationCode = request.ApplicationCode;
            userAccount.IsActive = request.IsActive;
            userAccount.ModifiedDate = DateTime.Now;
            userAccount.ModifiedBy = "System";

            await _unitOfWork.UserAccountRepository.UpdateAsync(userAccount);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = userAccount
            };
        }
    }
}