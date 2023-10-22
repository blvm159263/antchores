using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.CacheService
{
    public interface ICacheService
    {
        T GetData<T>(string key);

        void SetData<T>(string key, T value);

        object RemoveData(string key);
    }
}
