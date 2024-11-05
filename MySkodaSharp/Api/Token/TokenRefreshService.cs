using Microsoft.Extensions.Logging;
using MySkodaSharp.Api.VwIdentity.Models;
using MySkodaSharp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Token
{
    internal class TokenRefreshService
    {
        private const string BASE_URL = "https://mysmob.api.connect.skoda-auto.cz";
        private const string CODE_EXCHANGE_URL = BASE_URL + "/api/v1/authentication/exchange-authorization-code?tokenType=CONNECT";
        private const string REFRESH_TOKEN_URL = BASE_URL + "/api/v1/authentication/refresh-token?tokenType=CONNECT";

        private readonly ILogger _logger;
        private readonly HttpClient _client;

        private VagToken _vagToken;

        public TokenRefreshService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TokenRefreshService>();
            _client = new HttpClient();
        }

        public async Task<VagToken> Exchange(RequestParams query)
        {
            if (!query.Keys.Contains("id_token") || !query.Keys.Contains("code"))
            {
                throw new ArgumentException("Missing required parameters: id_token, code");
            }

            var data = new
            {
                code = query["code"],
                redirectUri = "myskoda://redirect/login/",
                verifier = query["code_verifier"]
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(CODE_EXCHANGE_URL, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var skodaToken = JsonConvert.DeserializeObject<SkodaToken>(responseBody);
                _vagToken = skodaToken.ToVagToken();

                _logger.LogTrace(JsonConvert.SerializeObject(_vagToken));
                _logger.LogDebug($"Token fetched: Expires {_vagToken.Expiry}");

                return _vagToken;
            }

            return null;
        }

        public async Task<VagToken> Refresh(VagToken token)
        {
            if (_vagToken != null && !_vagToken.IsExpired)
                return _vagToken;

            var data = new { token = token.RefreshToken };
            var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(REFRESH_TOKEN_URL, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var skodaToken = JsonConvert.DeserializeObject<SkodaToken>(responseBody);
                _vagToken = skodaToken.ToVagToken();

                _logger.LogTrace(JsonConvert.SerializeObject(_vagToken));
                _logger.LogDebug($"Token refreshed: Expires {_vagToken.Expiry}");

                return _vagToken;
            }

            return null;
        }

        public Func<Task<VagToken>> TokenSource(VagToken token)
        {
            return async () => await Refresh(token);
        }
    }
}
