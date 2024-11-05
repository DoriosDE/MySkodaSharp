using Microsoft.Extensions.Logging;
using MySkodaSharp.Api.VwIdentity;
using MySkodaSharp.Api.VwIdentity.Models;
using MySkodaSharp.Models;
using System;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Token
{
    internal class TokenService
    {
        private static RequestParams AUTH_PARAMS => new()
        {
            { "response_type", "code id_token" },
            { "client_id", "7f045eee-7003-4379-9968-9355ed2adb06@apps_vw-dilab_com" },
            { "redirect_uri", "myskoda://redirect/login/" },
            { "scope", "address badge birthdate cars driversLicense dealers email mileage mbb nationalIdentifier openid phone profession profile vin" }
        };

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public TokenService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<TokenService>();
        }

        public async Task<Func<Task<VagToken>>> TokenSource(string user, string password)
        {
            var vwIdentityService = new VwIdentityService(_loggerFactory);
            var authParams = await vwIdentityService.LoginAsync(AUTH_PARAMS, user, password);
            if (authParams == null)
            {
                throw new Exception("Login failed, unable to retrieve tokens");
            }

            // Token exchange using the TokenRefreshService
            var trs = new TokenRefreshService(_loggerFactory); // Initialize the TokenRefreshService
            var token = await trs.Exchange(authParams); // Exchange for tokens

            if (token == null)
            {
                throw new Exception("Token exchange failed");
            }

            // Return the token source function that can refresh tokens automatically
            return trs.TokenSource(token);
        }
    }
}
