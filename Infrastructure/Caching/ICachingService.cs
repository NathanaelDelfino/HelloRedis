using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloRedis.Infrastructure.Caching
{
    public interface ICachingService
    {
        Task SetAsync(string key, string value, TimeSpan? timeToLive = null);
        Task<string> GetAsync(string key);
    }
}