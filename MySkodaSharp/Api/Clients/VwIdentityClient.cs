using MySkodaSharp.Api.VwIdentity.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class VwIdentityClient : BaseClient
    {
        public async Task<string> Authorize(string url)
            => await GetAsync<string>(url);


        public async Task<string> Identify(string url, RequestParams requestParams)
            => await PostAsync<string>(url, new FormUrlEncodedContent(requestParams), true);

        public async Task<HttpResponseMessage> AcceptTermsAndConditions(string url, RequestParams requestParams)
            => await Client.PostAsync(url, new FormUrlEncodedContent(requestParams));

        public async Task<HttpResponseMessage> Authenticate(string url, RequestParams requestParams)
            => await Client.PostAsync(url, new FormUrlEncodedContent(requestParams));
    }
}
