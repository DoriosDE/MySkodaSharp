using MySkodaSharp.Enums;
using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class ActionConfig
    {
        [JsonProperty("mode")]
        public ChargeMode Mode { get; set; } // Charge Mode

        [JsonProperty("priority")]
        public int Priority { get; set; } // Priority

        [JsonProperty("minCurrent")]
        public double MinCurrent { get; set; } // Minimum Current

        [JsonProperty("maxCurrent")]
        public double MaxCurrent { get; set; } // Maximum Current
    }
}
