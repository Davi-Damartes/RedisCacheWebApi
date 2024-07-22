using StackExchange.Redis;
using System.Text.Json;

namespace WebApiCaching.Service
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;

        public CacheService( )
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string Key)
        {
            var value = _cacheDb.StringGet(Key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default!;
        }

        public object RemoveData(string Key)
        {
            var _exist = _cacheDb.KeyExists(Key);

            if (_exist)
                return _cacheDb.KeyDelete(Key);

            return false;
        }

        public bool SetData<T>(string Key, T value, DateTimeOffset expirationTime)
        {
            var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);

            var isSet = _cacheDb.StringSet(Key, JsonSerializer.Serialize(value), expirtyTime);

            return isSet;
        }
    }
}
