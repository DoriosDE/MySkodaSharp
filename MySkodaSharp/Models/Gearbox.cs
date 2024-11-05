using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class Gearbox
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
