using Newtonsoft.Json;

namespace MySkodaSharp.Models
{
    public class SkodaToken
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        public VagToken ToVagToken()
        {
            return new VagToken
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken,
                IdToken = IdToken
            };
        }
    }
}
