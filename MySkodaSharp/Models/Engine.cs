using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class Engine
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("powerInKW")]
        public int PowerInKW { get; set; }
    }
}