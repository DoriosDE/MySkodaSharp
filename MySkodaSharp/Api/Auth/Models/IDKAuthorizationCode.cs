using Newtonsoft.Json;

namespace MySkodaSharp.Api.Auth.Models
{
    internal class IDKAuthorizationCode
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
