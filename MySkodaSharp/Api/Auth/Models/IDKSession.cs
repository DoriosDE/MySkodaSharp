using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MySkodaSharp.Api.Auth.Models
{
    internal class IDKSession
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        public DateTimeOffset Expiry
            => GetExpiry();

        public bool IsExpired
            => DateTime.UtcNow > Expiry.AddMinutes(-10).UtcDateTime;

        private DateTimeOffset GetExpiry()
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(AccessToken);
            var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(token.Claims.First(x => x.Type == "exp").Value));
            return expiry;
        }
    }
}
