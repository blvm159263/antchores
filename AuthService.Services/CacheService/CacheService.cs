using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public T GetData<T>(string key)
        {
            var value = _distributedCache.GetString(key);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var _exist = _distributedCache.GetString(key);

            if (!string.IsNullOrEmpty(_exist))
            {
                _distributedCache.Remove(key);
                return true;
            }

            return false;
        }

        public void SetData<T>(string key, T value)
        {
            _distributedCache.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}
