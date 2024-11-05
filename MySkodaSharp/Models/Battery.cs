using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class Battery
    {
        [JsonProperty("capacityInKWh")]
        public int CapacityInKWh { get; set; }
    }
}