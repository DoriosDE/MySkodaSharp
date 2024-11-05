using Newtonsoft.Json;

namespace MySkodaSharp.Models.Responses
{
    /// <summary>
    /// SettingsResponse is the /v1/charging/<vin>/settings API response
    /// </summary>
    public class SettingsResponse : BaseResponse
    {
        [JsonProperty("autoUnlockPlugWhenCharged")]
        public string AutoUnlockPlugWhenCharged { get; set; }

        [JsonProperty("maxChargeCurrentAc")]
        public string MaxChargeCurrentAc { get; set; }

        [JsonProperty("targetStateOfChargeInPercent")]
        public int TargetStateOfChargeInPercent { get; set; }
    }
}
