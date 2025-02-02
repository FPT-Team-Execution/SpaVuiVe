using StackExchange.Redis;
using System.Text.Json;


namespace SkincareProductSalesSystem.Services.ExtendServices
{

    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        public CacheService() 
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }
        T ICacheService.GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        List<T> ICacheService.GetListData<T>(string key)
        {
            throw new NotImplementedException();
        }

        object ICacheService.RemoveData(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if (exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        bool ICacheService.SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expirtyTime);
        }

        bool ICacheService.SetListData<T>(string key, List<T> value, DateTimeOffset expirationTime)
        {
            throw new NotImplementedException();
        }
    }
}
