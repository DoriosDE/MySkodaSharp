using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v1/charging endpoint

    public class Battery
    {
        [JsonProperty("remainingCruisingRangeInMeters")]
        public int RemainingCruisingRangeInMeters { get; set; }

        [JsonProperty("stateOfChargeInPercent")]
        public int StateOfChargeInPercent { get; set; }
    }

    public class Charging
    {
        [JsonProperty("carCapturedTimestamp")]
        public DateTime? CarCapturedTimestamp { get; set; }

        [JsonProperty("errors")]
        public List<ChargingError> Errors { get; set; }

        [JsonProperty("isVehicleInSavedLocation")]
        public bool IsVehicleInSavedLocation { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }
        [JsonProperty("status")]
        public ChargingStatus Status { get; set; }
    }

    public class ChargingError
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public ChargingErrorType Type { get; set; }
    }

    public class ChargingStatus
    {
        [JsonProperty("battery")]
        public Battery Battery { get; set; }

        [JsonProperty("chargePowerInKw")]
        public double? ChargePowerInKw { get; set; }

        [JsonProperty("chargeType")]
        public ChargeType? ChargeType { get; set; }

        [JsonProperty("chargingRateInKilometersPerHour")]
        public double? ChargingRateInKilometersPerHour { get; set; }

        [JsonProperty("errors")]
        public List<ChargingError> Errors { get; set; }

        [JsonProperty("remainingTimeToFullyChargedInMinutes")]
        public int? RemainingTimeToFullyChargedInMinutes { get; set; }

        [JsonProperty("state")]
        public ChargingState? State { get; set; }
    }

    public class Settings
    {
        [JsonProperty("autoUnlockPlugWhenCharged")]
        public PlugUnlockMode? AutoUnlockPlugWhenCharged { get; set; }

        [JsonProperty("availableChargeModes")]
        public List<ChargeMode> AvailableChargeModes { get; set; }

        [JsonProperty("batterySupport")]
        public EnabledState? BatterySupport { get; set; }

        [JsonProperty("chargingCareMode")]
        public ActiveState? ChargingCareMode { get; set; }

        [JsonProperty("maxChargeCurrentAc")]
        public MaxChargeCurrent? MaxChargeCurrentAc { get; set; }
        [JsonProperty("preferredChargeMode")]
        public ChargeMode? PreferredChargeMode { get; set; }

        [JsonProperty("targetStateOfChargeInPercent")]
        public int? TargetStateOfChargeInPercent { get; set; }
    }
}
