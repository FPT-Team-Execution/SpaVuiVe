using AutoMapper;
using Azure.Identity;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Helpers;
using SkincareProductSalesSystem.Services.Models.AuthModels;
using SkincareProductSalesSystem.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Services
{
	public class AuthService : IAuthService
	{
		private UnitOfWork _uOW;
		private readonly PasswordHelper _passwordHelper;
		private readonly JwtHelper _jwtHelper;
		private IMapper _mapper;

		public AuthService(UnitOfWork uOW, PasswordHelper pwdHelper, IMapper mapper, JwtHelper jwtHelper)
		{
			_uOW = uOW;
			_passwordHelper = pwdHelper;
			_mapper = mapper;
			_jwtHelper = jwtHelper;
		}

		public async Task<RegisterModel> Register(RegisterModel dto)
		{
			try
			{
				if (await _uOW.UserRepository.CheckUsernameExistsAsync(dto.Username) != null)
					return new RegisterModel()
					{
						IsSuccess = false,
						Message = "Username already exists"
					};
				if (await _uOW.UserRepository.CheckEmailExistsAsync(dto.Email) != null)
					return new RegisterModel()
					{
						IsSuccess = false,
						Message = "Email already exists"
					};
				if (await _uOW.UserRepository.CheckPhoneNumberExistsAsync(dto.PhoneNumber) != null)
					return new RegisterModel()
					{
						IsSuccess = false,
						Message = "Phone Number already exists"
					};

				string passwordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
				string hashedPassword = _passwordHelper.HashPassword(dto.Password, passwordSalt);

				User user = _mapper.Map<User>(dto);
				user.PasswordHash = hashedPassword;
				user.PasswordSalt = passwordSalt;
				await _uOW.UserRepository.CreateAsync(user);

				return new RegisterModel()
				{
					IsSuccess = true,
				};
			}
			catch (Exception ex)
			{
				return new RegisterModel()
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<LoginResponseModel> LoginByUsername(LoginRequestModel dto)
		{
			try
			{
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(dto.Username);
				if (user == null)
					return new LoginResponseModel()
					{
						IsSuccess = false,
						Message = "Username not found"
					};
				if (!_passwordHelper.HashPassword(dto.Password, user.PasswordSalt).Equals(user.PasswordHash))
					return new LoginResponseModel()
					{
						IsSuccess = false,
						Message = "Incorrect Password"
					};

				string jwtToken = await _jwtHelper.GenerateAccessTokenAsync(user);
				string refreshToken = await _jwtHelper.GenerateRefreshTokenAsync();
				user.RefreshToken = refreshToken;

				await _uOW.UserRepository.UpdateAsync(user);
				return new LoginResponseModel()
				{
					IsSuccess = true,
					AccessToken = jwtToken,
					RefreshToken = refreshToken
				};
			}
			catch (Exception ex)
			{
				return new LoginResponseModel()
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ForgotPasswordResponseModel> ForgotPassword(string username)
		{
			try
			{
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(username);
				if (user == null)
					return new ForgotPasswordResponseModel()
					{
						IsSuccess = false,
						Message = "Username not found"
					};

				var passwordResetKey = _passwordHelper.CreateResetPasswordKey(username);
				if (string.IsNullOrEmpty(passwordResetKey))
					return new ForgotPasswordResponseModel()
					{
						IsSuccess = false,
						Message = "Cannot generate reset key."
					};
				return new ForgotPasswordResponseModel()
				{
					IsSuccess = true,
					Key = passwordResetKey
				};
			}
			catch (Exception ex)
			{
				return new ForgotPasswordResponseModel()
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<ResetPasswordResponseModel> ResetPassword(ResetPasswordRequestModel dto)
		{
				try
				{
					bool result = _passwordHelper.VerifyResetPasswordKey(dto.Username, dto.Key);
					if (!result)
					{
						return new ResetPasswordResponseModel()
						{
							IsSuccess = false,
							Message = "Incorrect Security Key"
						};
					}
					var user = await _uOW.UserRepository.CheckUsernameExistsAsync(dto.Username);
					if (user == null)
						return new ResetPasswordResponseModel()
						{
							IsSuccess = false,
							Message = "Username not found"
						};
					string passwordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
					string hashedPassword = _passwordHelper.HashPassword(dto.NewPassword, passwordSalt);

					user.PasswordHash = hashedPassword;
					user.PasswordSalt = passwordSalt;
					await _uOW.UserRepository.UpdateAsync(user);

					return new ResetPasswordResponseModel()
					{
						IsSuccess = true,
					};
				}
				catch (Exception ex)
				{
					return new ResetPasswordResponseModel()
					{
						IsSuccess = false,
						Message = ex.Message
					};
				}
		}
	}
}
