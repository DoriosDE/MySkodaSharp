using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    // Common models used in multiple responses

    public class Address
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("houseNumber")]
        public string HouseNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
