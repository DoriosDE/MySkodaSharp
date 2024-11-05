using Newtonsoft.Json;

namespace MySkodaSharp.Models.Responses
{
    /// <summary>
    /// StatusResponse is the /v2/vehicle-status/<vin> API response
    /// </summary>
    public class StatusResponse : BaseResponse
    {
        [JsonProperty("mileageInKm")]
        public double MileageInKm { get; set; }
    }
}
