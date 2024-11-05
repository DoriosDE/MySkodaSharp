using MySkodaSharp.Api.Clients;
using MySkodaSharp.Enums;
using MySkodaSharp.Models;
using MySkodaSharp.Models.Responses;
using System;
using System.Threading.Tasks;

namespace MySkodaSharp.Provider
{
    public class VehicleProvider
    {
        private readonly Func<string, string, Task> _action;
        private readonly Func<Task<ChargerResponse>> _chargerCached;
        private readonly Func<Task<ClimaterResponse>> _climateCached;
        private readonly Func<Task<SettingsResponse>> _settingsCached;
        private readonly Func<Task<StatusResponse>> _statusCached;
        private readonly Func<Task<VehicleResponse>> _vehicle;
        private readonly Func<Task> _wakeup;

        internal VehicleProvider(MySkodaApiClient mySkodaApiClient, string vin, TimeSpan cacheDuration)
        {
            _action = (action, value) => mySkodaApiClient.ExecuteActionAsync(vin, action, value);
            _chargerCached = ProviderHelper.Cached(() => mySkodaApiClient.GetChargerAsync(vin), cacheDuration);
            _climateCached = ProviderHelper.Cached(() => mySkodaApiClient.GetClimaterAsync(vin), cacheDuration);
            _settingsCached = ProviderHelper.Cached(() => mySkodaApiClient.GetSettingsAsync(vin), cacheDuration);
            _statusCached = ProviderHelper.Cached(() => mySkodaApiClient.GetStatusAsync(vin), cacheDuration);
            _vehicle = ProviderHelper.Cached(() => mySkodaApiClient.GetVehicleAsync(vin), cacheDuration);
            _wakeup = () => mySkodaApiClient.WakeupAsync(vin);
        }

        public async Task DisableChargingAsync()
        {
            await _action("charging", "stop");
        }

        public async Task EnableChargingAsync()
        {
            await _action("charging", "start");
        }

        public async Task<ChargeStatus> GetChargeStatusAsync()
        {
            var status = ChargeStatus.StatusA;

            var climateRes = await _climateCached();
            if (climateRes?.ChargerConnectionState == "CONNECTED")
            {
                status = ChargeStatus.StatusB;
            }

            var chargerRes = await _chargerCached();
            if (chargerRes?.Status?.State == "CHARGING")
            {
                status = ChargeStatus.StatusC;
            }

            return status;
        }

        public async Task<DateTime> GetChargingCompletedDateTimeAsync()
        {
            var res = await _chargerCached();
            var crg = res.Status;

            if (crg.State == "Error" || crg.ChargeType == "Invalid")
            {
                throw new InvalidOperationException("Not Available");
            }

            var remaining = TimeSpan.FromMinutes(crg.RemainingTimeToFullyChargedInMinutes);
            return DateTime.Now.Add(remaining);
        }

        public async Task<double> GetOdometerAsync()
        {
            var res = await _statusCached();
            return res?.MileageInKm ?? 0;
        }

        public async Task<long> GetRangeInKilometersAsync()
        {
            var res = await _chargerCached();
            return res?.Status?.Battery?.RemainingCruisingRangeInMeters / 1000 ?? 0;
        }

        public async Task<double> GetStateOfChargeInPercentAsync()
        {
            var res = await _chargerCached();
            return res?.Status?.Battery?.StateOfChargeInPercent ?? 0;
        }

        public async Task<long> GetStateOfChargeLimitAsync()
        {
            var res = await _chargerCached();
            return res?.Settings?.TargetStateOfChargeInPercent ?? 0;
        }

        public async Task<VehicleResponse> GetVehicleAsync()
        {
            return await _vehicle();
        }

        public async Task WakeupAsync()
        {
            await _wakeup();
        }
    }
}
