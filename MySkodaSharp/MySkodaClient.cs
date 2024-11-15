using Microsoft.Extensions.Logging;
using MySkodaSharp.Api;
using MySkodaSharp.Api.Auth;
using MySkodaSharp.Api.Clients;
using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Enums;
using MySkodaSharp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySkodaSharp
{
    public class MySkodaClient
    {
        private readonly ILoggerFactory _loggerFactory;

        private Authorization _authorization;

        public MySkodaClient(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task InitializeAsync(string email, string password, int timeoutInSeconds = 30, int cacheDurationInMinutes = 10)
        {
           _authorization = new(_loggerFactory);
            await _authorization.AuthorizeAsync(email, password);

            ApiClientFactory.Initialize(_authorization.GetAccessTokenAsync, TimeSpan.FromSeconds(timeoutInSeconds));

            VehicleCacheManager.Initialize(_loggerFactory, TimeSpan.FromMinutes(cacheDurationInMinutes));
        }

        public async Task<string> GetAuthTokenAsync()
            => await _authorization?.GetAccessTokenAsync();

        public async Task<Cache<AirConditioning>> GetAirConditioningAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<AirConditioning>();

        public async Task<Cache<Charging>> GetChargingAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<Charging>();

        public async Task<Cache<DrivingRange>> GetDrivingRangeAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<DrivingRange>();

        public async Task<Cache<Health>> GetHealthAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<Health>();

        public async Task<Cache<Info>> GetInfoAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<Info>();

        public async Task<Cache<Maintenance>> GetMaintenanceAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<Maintenance>();

        public async Task<Cache<Positions>> GetPositionsAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<Positions>();

        public async Task<Cache<Status>> GetStatusAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<Status>();

        public async Task<Cache<TripStatistics>> GetTripStatisticsAsync(string vin)
            => await VehicleCacheManager.Get(vin).GetCachedResultAsync<TripStatistics>();

        public async Task<Cache<User>> GetUserAsync()
            => await VehicleCacheManager.Get().GetCachedResultAsync<User>();

        public async Task<Cache<List<GarageEntry>>> GetGarageEntriesAsync()
        {
            var garage = await VehicleCacheManager.Get().GetCachedResultAsync<Garage>();
            return new(garage.CacheTime, garage.Result?.Vehicles);
        }

        public async Task<Cache<List<string>>> ListVehicleVinsAsync()
        {
            var garageEntries = await GetGarageEntriesAsync();
            return new(garageEntries.CacheTime, garageEntries.Result?.Select(vehicle => vehicle.Vin).ToList());
        }

        public async Task<Vehicle> GetVehicleAsync(string vin)
        {
            var info = await GetInfoAsync(vin);
            var maintenance = await GetMaintenanceAsync(vin);

            var vehicle = new Vehicle(info, maintenance);

            if (vehicle.IsCapabilityAvailable(CapabilityId.STATE))
            {
                vehicle.Status = await GetStatusAsync(vin);
                vehicle.DrivingRange = await GetDrivingRangeAsync(vin);
            }

            if (vehicle.IsCapabilityAvailable(CapabilityId.AIR_CONDITIONING))
            {
                vehicle.AirConditioning = await GetAirConditioningAsync(vin);
            }

            if (vehicle.IsCapabilityAvailable(CapabilityId.PARKING_POSITION))
            {
                vehicle.Positions = await GetPositionsAsync(vin);
            }

            if (vehicle.IsCapabilityAvailable(CapabilityId.TRIP_STATISTICS))
            {
                vehicle.TripStatistics = await GetTripStatisticsAsync(vin);
            }

            if (vehicle.IsCapabilityAvailable(CapabilityId.CHARGING))
            {
                vehicle.Charging = await GetChargingAsync(vin);
            }

            if (vehicle.IsCapabilityAvailable(CapabilityId.VEHICLE_HEALTH_INSPECTION))
            {
                vehicle.Health = await GetHealthAsync(vin);
            }

            return vehicle;
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            var vins = await ListVehicleVinsAsync();
            return (await Task.WhenAll(vins.Result?.Select(GetVehicleAsync)))?.ToList();
        }

        public async Task<FixtureReportGet> GenerateFixtureReportAsync(string vin, FixtureVehicle vehicle, Endpoint endpoint)
        {
            try
            {
                var result = await GetEndpointAsync(vin, endpoint, anonymize: true);

                return new()
                {
                    Type = FixtureReportType.GET,
                    VehicleId = vehicle.Id,
                    Success = true,
                    Endpoint = endpoint,
                    Raw = result.Raw,
                    Url = result.Url,
                    Result = JObject.FromObject(result.Result).ToObject<Dictionary<string, object>>()
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Type = FixtureReportType.GET,
                    VehicleId = vehicle.Id,
                    Success = false,
                    Endpoint = endpoint,
                    Error = Anonymizer.AnonymizeUrl(ex.ToString())
                };
            }
        }

        public async Task<GetEndpointResult<object>> GetEndpointAsync(string vin, Endpoint endpoint, bool anonymize = false)
        {
            switch (endpoint)
            {
                case Endpoint.INFO:
                    var info = await ApiClientFactory.GetClient<InfoClient>().GetInfoAsync(vin, anonymize);
                    return new(info?.Url, info?.Raw, info?.Result);

                case Endpoint.STATUS:
                    var status = await ApiClientFactory.GetClient<StatusClient>().GetStatusAsync(vin, anonymize);
                    return new(status?.Url, status?.Raw, status?.Result);

                case Endpoint.AIR_CONDITIONING:
                    var airConditioning = await ApiClientFactory.GetClient<AirConditioningClient>().GetAirConditioningAsync(vin, anonymize);
                    return new(airConditioning?.Url, airConditioning?.Raw, airConditioning?.Result);

                case Endpoint.POSITIONS:
                    var positions = await ApiClientFactory.GetClient<PositionsClient>().GetPositionsAsync(vin, anonymize);
                    return new(positions?.Url, positions?.Raw, positions?.Result);

                case Endpoint.HEALTH:
                    var health = await ApiClientFactory.GetClient<HealthClient>().GetHealthAsync(vin, anonymize);
                    return new(health?.Url, health?.Raw, health?.Result);

                case Endpoint.CHARGING:
                    var charging = await ApiClientFactory.GetClient<ChargingClient>().GetChargingAsync(vin, anonymize);
                    return new(charging?.Url, charging?.Raw, charging?.Result);

                case Endpoint.MAINTENANCE:
                    var maintenance = await ApiClientFactory.GetClient<MaintenanceClient>().GetMaintenanceAsync(vin, anonymize);
                    return new(maintenance?.Url, maintenance?.Raw, maintenance?.Result);

                case Endpoint.DRIVING_RANGE:
                    var drivingRange = await ApiClientFactory.GetClient<DrivingRangeClient>().GetDrivingRangeAsync(vin, anonymize);
                    return new(drivingRange?.Url, drivingRange?.Raw, drivingRange?.Result);

                case Endpoint.TRIP_STATISTICS:
                    var tripStatistics = await ApiClientFactory.GetClient<TripStatisticsClient>().GetTripStatisticsAsync(vin, anonymize);
                    return new(tripStatistics?.Url, tripStatistics?.Raw, tripStatistics?.Result);

                default:
                    throw new Exception("Endpoint not implemented.");
            }
        }

        public async Task<Fixture> GenerateGetFixture(string name, string description, List<string> vins, Endpoint endpoint)
        {
            var vehicleTasks = vins.Select(async (vin, i) =>
            {
                var info = await GetInfoAsync(vin);
                var fixtureVehicle = FixtureVehicle.CreateFixtureVehicle(i, info.Result);
                return (vin, fixtureVehicle);
            });

            var vehicles = await Task.WhenAll(vehicleTasks);

            var endpoints = (endpoint != Endpoint.ALL)
                ? new List<Endpoint> { endpoint }
                : Enum.GetValues(typeof(Endpoint)).Cast<Endpoint>().Where(ep => ep != Endpoint.ALL).ToList();

            var reportTasks = vehicles
                .SelectMany(vehicle =>
                    endpoints.Select(ep => GenerateFixtureReportAsync(vehicle.vin, vehicle.fixtureVehicle, ep))
                );

            var reports = await Task.WhenAll(reportTasks);

            return new()
            {
                Name = name,
                Description = description,
                GenerationTime = DateTime.UtcNow,
                Vehicles = vehicles.Select(vehicle => vehicle.fixtureVehicle).ToList(),
                Reports = reports.ToList()
            };
        }
    }
}
