using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User?> GetUserById(string id);
        Task<User?> CreateUser(User user);
        Task<User?> UpdateUser(User user);
        Task<User?> DeleteUser(string id);
    }
    public class UserService : IUserService
    {
        public Task<User?> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User?> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
