using AutoMapper;
using Azure.Identity;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using Protos.AuthService;
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


	public class AuthService2 : AuthServiceGRPC.AuthServiceGRPCBase
	{
		private UnitOfWork _uOW;
		private readonly JwtHelper _jwtHelper;
		private IMapper _mapper;

		public AuthService2(UnitOfWork uOW, IMapper mapper, JwtHelper jwtHelper) 
		{
			_uOW = uOW;
			_mapper = mapper;
			_jwtHelper = jwtHelper;
		}

		public override async Task<ServiceResultProto> Register(RegisterRequestProto request, ServerCallContext context)
		{
			try
			{
				if (await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username) != null)
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Tên tài khoản đã tồn tại."
					};
				if (await _uOW.UserRepository.CheckEmailExistsAsync(request.Email) != null)
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Email đã tồn tại"
					};
				if (await _uOW.UserRepository.CheckPhoneNumberExistsAsync(request.PhoneNumber) != null)
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Số điện thoại đã tồn tại"
					};

				string passwordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
				string hashedPassword = PasswordHelper.HashPassword(request.Password, passwordSalt);

				User user = _mapper.Map<User>(request);
				user.PasswordHash = hashedPassword;
				user.PasswordSalt = passwordSalt;
				await _uOW.UserRepository.CreateAsync(user);

				return new ServiceResultProto()
				{
					Status = 200,
					Message = "Thành công"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResultProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public override async Task<ServiceResulLoginProto> Login(LoginRequestProto request, ServerCallContext context)
		{
			try
			{
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username);
				if (user == null)
					return new ServiceResulLoginProto()
					{
						Status = 500,
						Message = "Không tìm thấy tên người dùng"
					};
				if (!PasswordHelper.HashPassword(request.Password, user.PasswordSalt).Equals(user.PasswordHash))
					return new ServiceResulLoginProto()
					{
						Status = 500,
						Message = "Sai mật khẩu"
					};

				string jwtToken = await _jwtHelper.GenerateAccessTokenAsync(user);
				string refreshToken = await _jwtHelper.GenerateRefreshTokenAsync();
				user.RefreshToken = refreshToken;

				await _uOW.UserRepository.UpdateAsync(user);
				return new ServiceResulLoginProto()
				{
					Status = 200,
					Message = "Thành công",
					Data = new LoginResponseProto()
					{
						AccessToken = jwtToken,
						RefreshToken = refreshToken
					}
				};
			}
			catch (Exception ex)
			{
				return new ServiceResulLoginProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public override async Task<ServiceResultProto> ForgotPassword(ForgotPasswordRequestProto request, ServerCallContext context)
		{
			try
			{
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username);
				if (user == null)
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Username not found"
					};

				var passwordResetKey = PasswordHelper.CreateResetPasswordKey(request.Username);
				if (string.IsNullOrEmpty(passwordResetKey))
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Cannot generate security key."
					};
				return new ServiceResultProto()
				{
					Status = 200,
					Message = "Thành công",
				};
			}
			catch (Exception ex)
			{
				return new ServiceResultProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public override async Task<ServiceResultProto> ResetPassword(ResetPasswordRequestProto request, ServerCallContext context)
		{
			try
			{
				bool result = PasswordHelper.VerifyResetPasswordKey(request.Username, request.Key);
				if (!result)
				{
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Mã bảo mật không đúng"
					};
				}
				var user = await _uOW.UserRepository.CheckUsernameExistsAsync(request.Username);
				if (user == null)
					return new ServiceResultProto()
					{
						Status = 500,
						Message = "Không tìm thấy tên người dùng"
					};
				string passwordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
				string hashedPassword = PasswordHelper.HashPassword(request.Password, passwordSalt);

				user.PasswordHash = hashedPassword;
				user.PasswordSalt = passwordSalt;
				await _uOW.UserRepository.UpdateAsync(user);

				return new ServiceResultProto()
				{
					Status = 200,
					Message = "Thành công",
				};
			}
			catch (Exception ex)
			{
				return new ServiceResultProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}
	}
}
