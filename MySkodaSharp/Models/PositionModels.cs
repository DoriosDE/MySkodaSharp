using MySkodaSharp.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MySkodaSharp.Models
{
    // Models for responses of api/v2/vehicle-status/{vin}/driving-range endpoint

    public class Error
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public ErrorType Type { get; set; }
    }

    public class Position
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("gpsCoordinates")]
        public Coordinates GpsCoordinates { get; set; }

        [JsonProperty("type")]
        public PositionType Type { get; set; }
    }
    public class Positions
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }

        [JsonProperty("positions")]
        public List<Position> PositionList { get; set; }
    }
}
