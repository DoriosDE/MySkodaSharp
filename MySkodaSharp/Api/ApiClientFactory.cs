using MySkodaSharp.Api.Clients;
using MySkodaSharp.Api.Handlers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api
{
    internal static class ApiClientFactory
    {
        private const string BASE_URL = "https://mysmob.api.connect.skoda-auto.cz";

        private static HttpClient _Client;

        public static void Initialize(Func<Task<string>> accessTokenSource, TimeSpan timeout)
        {
            _Client = new(new AccessTokenHandler(accessTokenSource))
            {
                BaseAddress = new(BASE_URL),
                Timeout = timeout
            };
        }

        public static TClient GetClient<TClient>() where TClient : BaseClient
            => (TClient)Activator.CreateInstance(typeof(TClient), _Client);
    }
}
