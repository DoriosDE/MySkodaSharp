using Microsoft.Extensions.Logging;
using MySkodaSharp.Api.Clients;
using MySkodaSharp.Api.Token;
using MySkodaSharp.Models;
using MySkodaSharp.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySkodaSharp
{
    public class MySkodaClient
    {
        private static readonly TimeSpan CACHE = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(30);

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<MySkodaClient> _logger;

        private MySkodaApiClient _mySkodaApiClient;

        public MySkodaClient(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<MySkodaClient>();
        }

        public async Task InitializeAsync(string user, string password)
        {
            var tokenService = new TokenService(_loggerFactory);
            var tokenSource = await tokenService.TokenSource(user, password);

            if (tokenSource == null)
            {
                throw new ArgumentNullException("Failed to obtain token source");
            }

            _mySkodaApiClient = new MySkodaApiClient(tokenSource, TIMEOUT);
        }

        public async Task<List<VehicleResponse>> GetVehiclesAsync()
        {
            if (_mySkodaApiClient == null)
            {
                throw new ArgumentNullException(nameof(_mySkodaApiClient));
            }

            return (await _mySkodaApiClient.GetGarageAsync())?.Vehicles;
        }

        public async Task<VehicleProvider> CreateVehicleProviderAsync(string vin)
        {
            var vehicle = await EnsureVehicleExists(vin, GetVehiclesAsync, v => v.Vin);
            if (vehicle != null)
            {
                return new VehicleProvider(_mySkodaApiClient, vehicle.Vin, CACHE);
            }
            return null;
        }

        private async Task<VehicleResponse> EnsureVehicleExists(string vin, Func<Task<List<VehicleResponse>>> getVehicles, Func<VehicleResponse, string> extract)
        {
            // Get the list of vehicles
            var vehicles = await getVehicles();
            if (vehicles == null)
            {
                throw new InvalidOperationException("Cannot get vehicles: list returned null.");
            }

            if (vehicles.Count() == 0)
            {
                throw new InvalidOperationException("Cannot get vehicles: list is empty.");
            }

            // If VIN is defined
            if (!string.IsNullOrWhiteSpace(vin))
            {
                vin = vin.ToUpper();
                foreach (var vehicle in vehicles)
                {
                    var vv = extract(vehicle);
                    if (string.Equals(vv.ToUpper(), vin, StringComparison.Ordinal))
                    {
                        return vehicle;
                    }
                }
            }
            else if (vehicles.Count() == 1)
            {
                // VIN is empty and exactly one vehicle
                return vehicles.First();
            }

            // If no matching vehicle is found
            var availableVins = vehicles.Select(v => extract(v)).ToList();
            throw new InvalidOperationException($"Cannot find vehicle, got: {string.Join(", ", availableVins)}");
        }
    }
}
