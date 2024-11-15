using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Handlers
{
    internal class AccessTokenHandler : DelegatingHandler
    {
        private readonly Func<Task<string>> _accessTokenSource;

        public AccessTokenHandler(Func<Task<string>> accessTokenSource)
        {
            _accessTokenSource = accessTokenSource ?? throw new ArgumentNullException(nameof(accessTokenSource));

            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _accessTokenSource();
            if (accessToken != null && !string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
