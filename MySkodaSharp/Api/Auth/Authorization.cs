using Microsoft.Extensions.Logging;
using MySkodaSharp.Api.Auth.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Auth
{
    internal class Authorization
    {
        private const string CLIENT_ID = "7f045eee-7003-4379-9968-9355ed2adb06@apps_vw-dilab_com";
        private const string BASE_URL_SKODA = "https://mysmob.api.connect.skoda-auto.cz";
        private const string BASE_URL_IDENT = "https://identity.vwgroup.io";

        private readonly SemaphoreSlim _refreshTokenLock = new(1, 1);

        private readonly ILogger<Authorization> _logger;
        private readonly HttpClient _httpClient;

        private IDKSession _idkSession;

        public Authorization(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Authorization>();
            _httpClient = new();
        }

        public async Task AuthorizeAsync(string email, string password)
        {
            try
            {
                _idkSession = await GetIDKSessionAsync(email, password);

                _logger.LogDebug($"Token fetched: Expires {_idkSession.Expiry}");
            }
            catch (Exception ex)
            {
                throw new Exception("Authorization failed", ex);
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_idkSession == null)
            {
                throw new Exception("Not authorized. Did you forget to call Authorization.Authorize()?");
            }

            if (_idkSession.IsExpired)
            {
                await RefreshTokenAsync();
            }
            return _idkSession.AccessToken;
        }

        private async Task<IDKSession> GetIDKSessionAsync(string email, string password)
        {
            var verifier = Utils.GenerateNonce();
            var csrf = await InitialOIDCAuthorizeAsync(verifier);
            csrf = await EnterEmailAddressAsync(csrf, email);
            var authCode = await EnterPasswordAsync(csrf, email, password);

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                code = authCode.Code,
                redirectUri = "myskoda://redirect/login/",
                verifier
            }), Encoding.UTF8, "application/json");

            var url = $"{BASE_URL_SKODA}/api/v1/authentication/exchange-authorization-code?tokenType=CONNECT";
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<IDKSession>(await response.Content.ReadAsStringAsync()) ?? throw new Exception("Failed to authorize.");
        }

        private async Task<CsrfState> InitialOIDCAuthorizeAsync(string verifier)
        {
            var verifierHash = SHA256.HashData(Encoding.UTF8.GetBytes(verifier));
            var challenge = Convert.ToBase64String(verifierHash).TrimEnd('=').Replace("+", "-").Replace("/", "_");

            var parameters = new Dictionary<string, string>
            {
                { "client_id", CLIENT_ID },
                { "nonce", Utils.GenerateNonce() },
                { "redirect_uri", "myskoda://redirect/login/" },
                { "response_type", "code id_token" },
                { "scope", "address badge birthdate cars driversLicense dealers email mileage mbb nationalIdentifier openid phone profession profile vin" },
                { "code_challenge", challenge },
                { "code_challenge_method", "s256" },
                { "prompt", "login" }
            };

            var url = QueryHelper.AddParameters($"{BASE_URL_IDENT}/oidc/v1/authorize", parameters);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return CsrfParser.ExtractCsrf(await response.Content.ReadAsStringAsync());
        }

        private async Task<CsrfState> EnterEmailAddressAsync(CsrfState csrf, string email)
        {
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "relayState", csrf.TemplateModel.RelayState },
                { "email", email },
                { "hmac", csrf.TemplateModel.Hmac },
                { "_csrf", csrf.Csrf }
            });

            var url = $"{BASE_URL_IDENT}/signin-service/v1/{CLIENT_ID}/login/identifier";
            var response = await _httpClient.PostAsync(url, formData);
            response.EnsureSuccessStatusCode();

            return CsrfParser.ExtractCsrf(await response.Content.ReadAsStringAsync());
        }

        private async Task<IDKAuthorizationCode> EnterPasswordAsync(CsrfState csrf, string email, string password)
        {
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "relayState", csrf.TemplateModel.RelayState },
                { "email", email },
                { "password", password },
                { "hmac", csrf.TemplateModel.Hmac },
                { "_csrf", csrf.Csrf }
            });

            var url = $"{BASE_URL_IDENT}/signin-service/v1/{CLIENT_ID}/login/authenticate";
            var response = await _httpClient.PostAsync(url, formData);

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                throw new Exception(response.ReasonPhrase);

            if ((await response.Content.ReadAsStringAsync()).Contains("content=\"termsAndConditions\""))
                 throw new Exception("Redirect to Terms and Conditions was encountered.");

            var location = response.Headers.Location?.ToString() ?? throw new Exception("Failed to authorize.");

            while (!location.StartsWith("myskoda://"))
            {
                response = await _httpClient.GetAsync(location);
                location = response.Headers.Location?.ToString() ?? throw new Exception("Failed to authorize.");
            }

            var parameters = QueryHelper.ParseQuery(new Uri(location).Fragment.TrimStart('#'));

            return new()
            {
                Code = parameters["code"],
                TokenType = parameters["token_type"],
                IdToken = parameters["id_token"]
            };
        }

        private async Task RefreshTokenAsync()
        {
            await _refreshTokenLock.WaitAsync();
            try
            {
                if (_idkSession == null || !_idkSession.IsExpired) return;

                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    token = _idkSession.RefreshToken
                }), Encoding.UTF8, "application/json");

                var url = $"{BASE_URL_SKODA}/api/v1/authentication/refresh-token?tokenType=CONNECT";
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                _idkSession = JsonConvert.DeserializeObject<IDKSession>(await response.Content.ReadAsStringAsync()) ?? throw new Exception("Failed to authorize.");

                _logger.LogDebug($"Token fetched: Expires {_idkSession.Expiry}");
            }
            catch (Exception ex)
            {
                throw new Exception("Token refresh failed.", ex);
            }
            finally
            {
                _refreshTokenLock.Release();
            }
        }
    }
}
