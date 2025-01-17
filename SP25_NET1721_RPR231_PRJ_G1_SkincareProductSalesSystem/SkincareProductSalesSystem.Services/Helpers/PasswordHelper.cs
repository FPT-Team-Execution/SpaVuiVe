using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Helpers
{
	public class PasswordHelper
	{
		private Dictionary<string, PasswordResetStruct> _passwordResetKeyStorage;

		public PasswordHelper()
		{
			if (_passwordResetKeyStorage == null) _passwordResetKeyStorage = new();
		}

		public string HashPassword(string password, string salt)
		{

			var saltBytes = Encoding.UTF8.GetBytes(salt);

			var hash = Rfc2898DeriveBytes.Pbkdf2(
		Encoding.UTF8.GetBytes(password),
		saltBytes,
		350000,
		HashAlgorithmName.SHA512,
		64);
			return Convert.ToHexString(hash);
		}

		public string? CreateResetPasswordKey(string userId)
		{
			try
			{
				string generatedKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8)).Substring(0, 5);
				_passwordResetKeyStorage[userId] = new PasswordResetStruct()
				{
					Key = generatedKey,
					Expiry = DateTime.UtcNow.AddDays(1)
				}
				return generatedKey;
			}
			catch (Exception ex) { return null; }
		}

		public bool VerifyResetPasswordKey(string userId, string key)
		{
			try
			{
				if (!(_passwordResetKeyStorage[userId].Key.Equals(key) && DateTime.UtcNow <= _passwordResetKeyStorage[userId].Expiry))
					return false; ;

				_passwordResetKeyStorage.Remove(userId);
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}

	internal struct PasswordResetStruct
	{
		public string Key {  get; set; }
		public DateTime Expiry { get; set; }
	}

}
