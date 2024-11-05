using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class BatteryStatus
    {
        [JsonProperty("remainingCruisingRangeInMeters")]
        public long RemainingCruisingRangeInMeters { get; set; }

        [JsonProperty("stateOfChargeInPercent")]
        public int StateOfChargeInPercent { get; set; }
    }
}
