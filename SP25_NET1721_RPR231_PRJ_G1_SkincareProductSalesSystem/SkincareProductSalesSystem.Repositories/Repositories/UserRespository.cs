
using SkincareProductSalesSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Models;
using System.ComponentModel;

namespace SkincareProductSalesSystem.Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
		public async Task<User?> AuthenticateByEmailAsync(string email, string hashedPassword)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.PasswordHash.Equals(hashedPassword));
		}

		public async Task<User?> AuthenticateByUsernameAsync(string username, string hashedPassword)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.PasswordHash.Equals(hashedPassword));
		}

		public async Task<User?> CheckEmailExistsAsync(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
		}

		public async Task<User?> CheckUsernameExistsAsync(string username)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
		}

		public async Task<User?> CheckPhoneNumberExistsAsync(string phoneNumber)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(phoneNumber));
		}

		
	}
}