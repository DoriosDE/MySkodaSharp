using Newtonsoft.Json;

namespace MySkodaSharp.Models.Responses
{
    /// <summary>
    /// ClimaterResponse is the /v2/air-conditioning/<vin> API response
    /// </summary>
    public class ClimaterResponse : BaseResponse
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("chargerConnectionState")]
        public string ChargerConnectionState { get; set; }

        [JsonProperty("chargerLockState")]
        public string ChargerLockState { get; set; }
    }
}
