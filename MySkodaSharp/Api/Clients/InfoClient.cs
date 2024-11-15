using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class InfoClient : BaseClient
    {
        public InfoClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Info>> GetInfoAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v2/garage/vehicles/{vin}?connectivityGenerations=MOD1&connectivityGenerations=MOD2&connectivityGenerations=MOD3&connectivityGenerations=MOD4";
            return await GetAsync<Info>(url, anonymize, Anonymizer.AnonymizeInfo);
        }
    }
}
