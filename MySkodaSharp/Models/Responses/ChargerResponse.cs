using Newtonsoft.Json;

namespace MySkodaSharp.Models.Responses
{
    /// <summary>
    /// ChargerResponse is the /v2/charging/<vin> API response
    /// </summary>
    public class ChargerResponse : BaseResponse
    {
        [JsonProperty("isVehicleInSaveLocation")]
        public bool IsVehicleInSaveLocation { get; set; }

        [JsonProperty("status")]
        public ChargerStatus Status { get; set; }

        [JsonProperty("settings")]
        public SettingsResponse Settings { get; set; }
    }
}
