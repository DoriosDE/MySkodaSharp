using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class Specification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("modelYear")]
        public string ModelYear { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("systemCode")]
        public string SystemCode { get; set; }

        [JsonProperty("systemModelId")]
        public string SystemModelId { get; set; }

        [JsonProperty("engine")]
        public Engine Engine { get; set; }

        [JsonProperty("battery")]
        public Battery Battery { get; set; }

        [JsonProperty("gearbox")]
        public Gearbox Gearbox { get; set; }

        [JsonProperty("trimLevel")]
        public string TrimLevel { get; set; }

        [JsonProperty("manufacturingDate")]
        public string ManufacturingDate { get; set; }

        [JsonProperty("maxChargingPowerInKW")]
        public int MaxChargingPowerInKW { get; set; }
    }
}
