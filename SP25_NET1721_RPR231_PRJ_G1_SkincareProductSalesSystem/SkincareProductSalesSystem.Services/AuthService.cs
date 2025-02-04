using AutoMapper;
using Azure.Identity;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using SkincareProductSalesSystem.Services.Helpers;
using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
	public interface IAuthService
	{
		Task<IServiceResult> Register(RegisterModel request);
		Task<IServiceResult> LoginByUsername(LoginRequestModel request);
		Task<IServiceResult> ForgotPassword(string username);
		Task<IServiceResult> ResetPassword(ResetPasswordRequestModel request);
	}

	public class AuthService : IAuthService
	{
		private UnitOfWork _uOW;
		private readonly JwtHelper _jwtHelper;
		private IMapper _mapper;

		public AuthService(UnitOfWork uOW, IMapper mapper, JwtHelper jwtHelper)
		{
			_uOW = uOW;
			_mapper = mapper;
			_jwtHelper = jwtHelper;
		}

		public async Task<IServiceResult> Register(RegisterModel request)
		{
			try
			{
				if (await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username) != null)
					return new ServiceResult()
					{
						Status = 500,
						Message = "Username already exists"
					};
				if (await _uOW.UserRepository.CheckEmailExistsAsync(request.Email) != null)
					return new ServiceResult()
					{
						Status = 500,
						Message = "Email already exists"
					};
				if (await _uOW.UserRepository.CheckPhoneNumberExistsAsync(request.PhoneNumber) != null)
					return new ServiceResult()
					{
						Status = 500,
						Message = "Phone Number already exists"
					};

				string passwordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
				string hashedPassword = PasswordHelper.HashPassword(request.Password, passwordSalt);

				User user = _mapper.Map<User>(request);
				user.PasswordHash = hashedPassword;
				user.PasswordSalt = passwordSalt;
				await _uOW.UserRepository.CreateAsync(user);

				return new ServiceResult()
				{
					Status = 200
				};
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> LoginByUsername(LoginRequestModel request)
		{
			try
			{
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username);
				if (user == null)
					return new ServiceResult()
					{
						Status = 500,
						Message = "Username not found"
					};
				if (!PasswordHelper.HashPassword(request.Password, user.PasswordSalt).Equals(user.PasswordHash))
					return new ServiceResult()
					{
						Status = 500,
						Message = "Incorrect Password"
					};

				string jwtToken = await _jwtHelper.GenerateAccessTokenAsync(user);
				string refreshToken = await _jwtHelper.GenerateRefreshTokenAsync();
				user.RefreshToken = refreshToken;

				await _uOW.UserRepository.UpdateAsync(user);
				return new ServiceResult()
				{
					Status = 200,
					Data = new LoginResponseModel
					{
						AccessToken = jwtToken,
						RefreshToken = refreshToken
					}
				};
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> ForgotPassword(string username)
		{
			try
			{
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(username);
				if (user == null)
					return new ServiceResult()
					{
						Status = 500,
						Message = "Username not found"
					};

				var passwordResetKey = PasswordHelper.CreateResetPasswordKey(username);
				if (string.IsNullOrEmpty(passwordResetKey))
					return new ServiceResult()
					{
						Status = 500,
						Message = "Cannot generate security key."
					};
				 return new ServiceResult()
				 {
					 Status = 204,
				 };
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public async Task<IServiceResult> ResetPassword(ResetPasswordRequestModel request)
		{
			try
			{
				bool result = PasswordHelper.VerifyResetPasswordKey(request.Username, request.Key);
				if (!result)
				{
					return new ServiceResult()
					{
						Status = 500,
						Message = "Incorrect Security Key"
					};
				}
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username);
				if (user == null)
					return new ServiceResult()
					{
						Status = 500,
						Message = "Username not found"
					};
				string passwordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
				string hashedPassword = PasswordHelper.HashPassword(request.NewPassword, passwordSalt);

				user.PasswordHash = hashedPassword;
				user.PasswordSalt = passwordSalt;
				await _uOW.UserRepository.UpdateAsync(user);

				return new ServiceResult()
				{
					Status = 200
				};
			}
			catch (Exception ex)
			{
				return new ServiceResult()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}
	}

	public class RegisterModel : BaseModel
	{
		[Required]
		public string? Username { get; set; }
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? FullName { get; set; }
		[Required]
		[RegularExpression(@"^0\d{9}$", ErrorMessage = "Invalid Phone Number format")]
		public string? PhoneNumber { get; set; }
		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string Password { get; set; }
	}

	public class LoginRequestModel : BaseModel
	{
		[Required]
		public string Username { get; set; }
		
		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string Password { get; set; }
	}

	public class LoginResponseModel : BaseModel
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}

	public class ResetPasswordRequestModel : BaseModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[RegularExpression(@"^.{5}$", ErrorMessage = "Incorrect Security Key")]
		public string Key { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string NewPassword { get; set; }
	}


}
