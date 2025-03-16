using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Helpers
{
	public class JwtHelper
	{
		private IConfiguration _configuration;

		public JwtHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<string> GenerateAccessTokenAsync(User user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.Username),
					new Claim(ClaimTypes.Name, user.FullName),
					new Claim("Phone", user.PhoneNumber),
					new Claim(ClaimTypes.Role, user.RoleName)   ,
					new Claim(ClaimTypes.Email, user.Email),
					new Claim("UserId", user.UserId)
				}),
				Expires = DateTime.UtcNow.AddMinutes(120),
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = jwtTokenHandler.WriteToken(jwtTokenHandler.CreateToken(tokenDescription));
			return token;
		}

		public async Task<string> GenerateRefreshTokenAsync()
		{
			var random = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(random); //dien so vao mang
				return Convert.ToBase64String(random); //chuyen mang byte thanh base64
			}
		}
	}
}
