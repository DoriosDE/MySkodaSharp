using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/vehicle-status/{vin} endpoint

    public class StatisticsEntry
    {
        [JsonProperty("averageAuxConsumption")]
        public float? AverageAuxConsumption { get; set; }

        [JsonProperty("averageElectricConsumption")]
        public float? AverageElectricConsumption { get; set; }

        [JsonProperty("averageFuelConsumption")]
        public float? AverageFuelConsumption { get; set; }

        [JsonProperty("averageRecuperation")]
        public float? AverageRecuperation { get; set; }

        [JsonProperty("averageSpeedInKmph")]
        public int? AverageSpeedInKmph { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("mileageInKm")]
        public int? MileageInKm { get; set; }

        [JsonProperty("travelTimeInMin")]
        public int? TravelTimeInMin { get; set; }

        [JsonProperty("tripIds")]
        public List<int> TripIds { get; set; }
    }

    public class TripStatistics
    {
        [JsonProperty("detailedStatistics")]
        public List<StatisticsEntry> DetailedStatistics { get; set; }

        [JsonProperty("overallAverageElectricConsumption")]
        public float? OverallAverageElectricConsumption { get; set; }

        [JsonProperty("overallAverageFuelConsumption")]
        public float? OverallAverageFuelConsumption { get; set; }

        [JsonProperty("overallAverageMileageInKm")]
        public int? OverallAverageMileageInKm { get; set; }

        [JsonProperty("overallAverageSpeedInKmph")]
        public int OverallAverageSpeedInKmph { get; set; }

        [JsonProperty("overallAverageTravelTimeInMin")]
        public int OverallAverageTravelTimeInMin { get; set; }

        [JsonProperty("overallMileageInKm")]
        public int OverallMileageInKm { get; set; }

        [JsonProperty("overallTravelTimeInMin")]
        public int OverallTravelTimeInMin { get; set; }

        [JsonProperty("vehicleType")]
        public VehicleType VehicleType { get; set; }
    }
}
