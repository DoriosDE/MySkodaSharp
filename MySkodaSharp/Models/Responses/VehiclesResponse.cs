using Newtonsoft.Json;
using System.Collections.Generic;

namespace MySkodaSharp.Models.Responses
{
    /// <summary>
    /// VehiclesResponse is the /v3/garage API response
    /// </summary>
    public class VehiclesResponse : BaseResponse
    {
        [JsonProperty("vehicles")]
        public List<VehicleResponse> Vehicles { get; set; }
    }
}
