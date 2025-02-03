using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.ExtendServices
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        List<T> GetListData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        bool SetListData<T>(string key, List<T> value, DateTimeOffset expirationTime);
        object RemoveData(string key);
    }
}
