using SkincareProductSalesSystem.Services.Models.AuthModels;

namespace SkincareProductSalesSystem.Services.Services.Interfaces
{
	public interface IAuthService
	{
		Task<RegisterModel> Register(RegisterModel dto);
		Task<LoginResponseModel> LoginByUsername(LoginRequestModel dto);
		Task<ForgotPasswordResponseModel> ForgotPassword(string username);
		Task<ResetPasswordResponseModel> ResetPassword(ResetPasswordRequestModel dto);
	}
}