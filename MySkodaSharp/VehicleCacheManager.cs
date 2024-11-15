using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace MySkodaSharp
{
    internal static class VehicleCacheManager
    {
        private static ILoggerFactory _loggerFactory;
        private static TimeSpan _cacheDuration;
        private static ConcurrentDictionary<string, VehicleCache> _dataProviders = new();

        public static void Initialize(ILoggerFactory loggerFactory, TimeSpan cacheDuration)
        {
            _loggerFactory = loggerFactory;
            _cacheDuration = cacheDuration;
        }

        public static VehicleCache Get(string vin = "", TimeSpan? cacheDuration = null)
        {
            var shortendVin = !string.IsNullOrWhiteSpace(vin) && vin.Length >= 5 ? vin[..^5] : vin;
            var logger = _loggerFactory.CreateLogger($"{typeof(VehicleCache).FullName}[\"{shortendVin}\"]");
            return _dataProviders.GetOrAdd(vin, _vin => new(logger, _vin, cacheDuration ?? _cacheDuration));
        }
    }
}
