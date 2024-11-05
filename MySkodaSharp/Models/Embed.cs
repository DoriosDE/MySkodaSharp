using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    public class Embed
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; private set; }

        [JsonProperty("capacity")]
        public double Capacity { get; private set; }

        [JsonProperty("phases")]
        public int Phases { get; private set; }

        [JsonProperty("identifiers")]
        public List<string> Identifiers { get; private set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; private set; }

        [JsonProperty("onIdentify")]
        public ActionConfig OnIdentify { get; private set; }

        public Embed()
        {
            Identifiers = new List<string>();
            Features = new List<Feature>();
        }

        public void FromVehicle(string title, double capacity)
        {
            if (string.IsNullOrEmpty(Title))
            {
                Title = title;
            }
            if (Capacity == 0)
            {
                Capacity = capacity;
            }
        }
    }
}
