using Grpc.Core;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.GrpcServices;

namespace SkincareProductSalesSystem.Services.GrpcServices
{
    public class UserAccountGrpcService : UserAccountGrpc.UserAccountGrpcBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountGrpcService(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public override async Task<UserAccountResponse> GetUserAccount(UserAccountRequest request, ServerCallContext context)
        {
            var result = await _userAccountService.GetAsync(request.Id);

            if (result.Status == 200 && result.Data != null)
            {
                var userAccount = (UserAccount)result.Data;
                return new UserAccountResponse
                {
                    Id = userAccount.UserAccountId,
                    UserName = userAccount.UserName,
                    FullName = userAccount.FullName,
                    Email = userAccount.Email,
                    Phone = userAccount.Phone
                };
            }

            throw new RpcException(new Status(StatusCode.NotFound, "User account not found"));
        }
    }
}