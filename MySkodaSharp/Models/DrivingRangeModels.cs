using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/vehicle-status/{vin}/driving-range endpoint

    public class DrivingRange
    {
        [JsonProperty("carCapturedTimestamp")]
        public DateTime CarCapturedTimestamp { get; set; }

        [JsonProperty("carType")]
        public EngineType CarType { get; set; }

        [JsonProperty("primaryEngineRange")]
        public EngineRange PrimaryEngineRange { get; set; }

        [JsonProperty("secondaryEngineRange")]
        public EngineRange SecondaryEngineRange { get; set; }

        [JsonProperty("totalRangeInKm")]
        public int TotalRangeInKm { get; set; }
    }

    public class EngineRange
    {
        [JsonProperty("currentFuelLevelInPercent")]
        public int? CurrentFuelLevelInPercent { get; set; }

        [JsonProperty("currentSoCInPercent")]
        public int CurrentSoCInPercent { get; set; }

        [JsonProperty("engineType")]
        public EngineType EngineType { get; set; }

        [JsonProperty("remainingRangeInKm")]
        public int RemainingRangeInKm { get; set; }
    }
}
