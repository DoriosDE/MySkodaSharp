using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/garage/vehicles/{vin}

    public class Garage
    {
        [JsonProperty("errors")]
        public List<GarageError> Errors { get; set; }

        [JsonProperty("vehicles")]
        public List<GarageEntry> Vehicles { get; set; }
    }

    public class GarageEntry
    {
        [JsonProperty("compositeRenders")]
        public List<CompositeRender> CompositeRenders { get; set; }

        [JsonProperty("devicePlatform")]
        public string DevicePlatform { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("renders")]
        public List<Render> Renders { get; set; }

        [JsonProperty("state")]
        public VehicleState State { get; set; }

        [JsonProperty("systemModelId")]
        public string SystemModelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("vin")]
        public string Vin { get; set; }
    }

    public class GarageError
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public GarageErrorType Type { get; set; }
    }
}
