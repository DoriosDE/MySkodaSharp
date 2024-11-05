using MySkodaSharp.Models;
using MySkodaSharp.Models.Responses;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class MySkodaApiClient : BaseClient
    {
        #region AccessTokenHandler
        private class AccessTokenHandler : DelegatingHandler
        {
            private readonly Func<Task<VagToken>> _trs;

            public AccessTokenHandler(Func<Task<VagToken>> trs)
            {
                _trs = trs ?? throw new ArgumentNullException(nameof(trs));

                InnerHandler = new HttpClientHandler();
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var vagToken = await _trs();
                if (vagToken != null && !string.IsNullOrEmpty(vagToken.AccessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", vagToken.AccessToken);
                }
                return await base.SendAsync(request, cancellationToken);
            }
        }
        #endregion AccessTokenHandler

        private const string BASE_URL = "https://mysmob.api.connect.skoda-auto.cz";

        public MySkodaApiClient(Func<Task<VagToken>> trs, TimeSpan timeout)
            : base()
        {
            Client = new HttpClient(new AccessTokenHandler(trs))
            {
                BaseAddress = new Uri(BASE_URL),
                Timeout = timeout
            };
        }

        /// <summary>
        /// Action executes a vehicle action
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <param name="action">The <see cref="string"/>.</param>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns></returns>
        public async Task ExecuteActionAsync(string vin, string action, string value)
            => await PostAsync<string>($"/api/v1/{action}/{vin}/{value}", null);

        /// <summary>
        /// Charger implements the /v1/charging/{vin} response
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <returns>The <see cref="ChargerResponse"/>.</returns>
        public async Task<ChargerResponse> GetChargerAsync(string vin)
            => await GetAsync<ChargerResponse>($"/api/v1/charging/{vin}");

        /// <summary>
        /// Climater implements the /v2/air-conditioning/{vin} response
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <returns>The <see cref="ClimaterResponse"/>.</returns>
        public async Task<ClimaterResponse> GetClimaterAsync(string vin)
            => await GetAsync<ClimaterResponse>($"/api/v2/air-conditioning/{vin}");

        /// <summary>
        /// Vehicles implements the /v2/garage response
        /// </summary>
        /// <returns>The <see cref="Garage"/>.</returns>
        public async Task<VehiclesResponse> GetGarageAsync()
            => await GetAsync<VehiclesResponse>("/api/v2/garage");

        /// <summary>
        /// Settings implements the /v1/charging/{vin} response
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <returns>The <see cref="SettingsResponse"/>.</returns>
        public async Task<SettingsResponse> GetSettingsAsync(string vin)
            => (await GetChargerAsync(vin))?.Settings;

        /// <summary>
        /// Status implements the /v1/vehicle-health-report/warning-lights/{vin} response
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <returns>The <see cref="StatusResponse"/>.</returns>
        public async Task<StatusResponse> GetStatusAsync(string vin)
            => await GetAsync<StatusResponse>($"/api/v1/vehicle-health-report/warning-lights/{vin}");

        /// <summary>
        /// VehicleDetails implements the /v2/garage/vehicles/{vin} response
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <returns>The <see cref="VehicleResponse"/>.</returns>
        public async Task<VehicleResponse> GetVehicleAsync(string vin)
            => await GetAsync<VehicleResponse>($"/api/v2/garage/vehicles/{vin}");

        /// <summary>
        /// Wakeup sends a wake-up call to the vehicle
        /// </summary>
        /// <param name="vin">The <see cref="string"/>.</param>
        /// <returns></returns>
        public async Task WakeupAsync(string vin)
            => await PostAsync<string>($"/api/v1/vehicle-wakeup/{vin}", null);
    }
}
