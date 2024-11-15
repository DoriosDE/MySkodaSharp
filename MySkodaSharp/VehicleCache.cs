using Microsoft.Extensions.Logging;
using MySkodaSharp.Api;
using MySkodaSharp.Api.Clients;
using MySkodaSharp.Models;
using MySkodaSharp.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MySkodaSharp
{
    internal class VehicleCache
    {
        private readonly ILogger _logger;
        private readonly Dictionary<Type, object> _cacheMap;

        internal VehicleCache(ILogger logger, string vin, TimeSpan cacheDuration)
        {
            _logger = logger;

            CacheHelper.Initialize(cacheDuration);

            _cacheMap = new()
            {
                { typeof(AirConditioning), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<AirConditioningClient>().GetAirConditioningAsync(vin)) },
                { typeof(Charging), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<ChargingClient>().GetChargingAsync(vin)) },
                { typeof(DrivingRange), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<DrivingRangeClient>().GetDrivingRangeAsync(vin)) },
                { typeof(Garage), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<GarageClient>().GetGarageAsync()) },
                { typeof(Health), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<HealthClient>().GetHealthAsync(vin)) },
                { typeof(Info), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<InfoClient>().GetInfoAsync(vin)) },
                { typeof(Maintenance), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<MaintenanceClient>().GetMaintenanceAsync(vin)) },
                { typeof(Positions), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<PositionsClient>().GetPositionsAsync(vin)) },
                { typeof(Status), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<StatusClient>().GetStatusAsync(vin)) },
                { typeof(TripStatistics), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<TripStatisticsClient>().GetTripStatisticsAsync(vin)) },
                { typeof(User), CacheHelper.GetOrUpdate(() => ApiClientFactory.GetClient<UserClient>().GetUserAsync()) },
            };
        }

        public async Task<Cache<T?>> GetCachedResultAsync<T>()
            => await LogException(_cacheMap.GetValueOrDefault(typeof(T)) as Func<Task<Cache<T>>>);

        private async Task<T> LogException<T>(Func<Task<T>> func, [CallerMemberName] string callerName = null)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.GetType().Name} in {callerName}");
                return default;
            }
        }
    }
}
