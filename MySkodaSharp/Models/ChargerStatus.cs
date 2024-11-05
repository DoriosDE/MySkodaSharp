using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class ChargerStatus
    {
        [JsonProperty("chargingRateInKilometersPerHour")]
        public double ChargingRateInKilometersPerHour { get; set; }

        [JsonProperty("chargePowerInKw")]
        public double ChargePowerInKw { get; set; }

        [JsonProperty("remainingTimeToFullyChargedInMinutes")]
        public long RemainingTimeToFullyChargedInMinutes { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("chargeType")]
        public string ChargeType { get; set; }

        [JsonProperty("battery")]
        public BatteryStatus Battery { get; set; }
    }
}
