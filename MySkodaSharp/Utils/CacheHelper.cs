using MySkodaSharp.Api.Models;
using MySkodaSharp.Models;
using System;
using System.Threading.Tasks;

namespace MySkodaSharp.Utils
{
    internal class CacheHelper
    {
        private static TimeSpan _cacheDuration;

        public static void Initialize(TimeSpan cacheDuration)
        {
            _cacheDuration = cacheDuration;
        }

        public static Func<Task<Cache<T>>> GetOrUpdate<T>(Func<Task<GetEndpointResult<T>>> func)
        {
            Cache<T> cache = new(DateTimeOffset.MinValue, default);

            return async () =>
            {
                if (DateTime.UtcNow - cache.CacheTime > _cacheDuration || cache.Result == null)
                {
                    cache.Result = (await func()).Result;
                    cache.CacheTime = DateTimeOffset.UtcNow;
                }
                return cache;
            };
        }
    }
}
