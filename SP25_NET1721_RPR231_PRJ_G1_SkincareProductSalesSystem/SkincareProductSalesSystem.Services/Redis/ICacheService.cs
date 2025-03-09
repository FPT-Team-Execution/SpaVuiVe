using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.ExtendServices
{
    public interface ICacheService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task<List<T>> GetListDataAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset? expirationTime = null);
        Task<bool> SetListDataAsync<T>(string key, List<T> value, DateTimeOffset? expirationTime = null);
        Task<object> RemoveDataAsync(string key);
    }
}
