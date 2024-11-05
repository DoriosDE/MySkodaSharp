using System;
using System.Threading.Tasks;

namespace MySkodaSharp.Provider
{
    // Helper class for caching
    internal class ProviderHelper
    {
        public static Func<Task<T>> Cached<T>(Func<Task<T>> func, TimeSpan cacheDuration)
        {
            T cachedValue = default;
            var cacheTime = DateTime.MinValue;

            return async () =>
            {
                if (DateTime.UtcNow - cacheTime > cacheDuration || cachedValue == null)
                {
                    cachedValue = await func();
                    cacheTime = DateTime.UtcNow;
                }
                return cachedValue;
            };
        }
    }
}
