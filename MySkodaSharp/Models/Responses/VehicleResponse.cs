using MySkodaSharp.Models.Responses;
using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class VehicleResponse : BaseResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("vin")]
        public string Vin { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } // User-given name

        [JsonProperty("lastUpdatedAt")]
        public string LastUpdatedAt { get; set; }

        [JsonProperty("specification")]
        public Specification Specification { get; set; }
    }
}
