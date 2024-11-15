using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class GarageClient : BaseClient
    {
        public GarageClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Garage>> GetGarageAsync(bool anonymize = false)
        {
            var url = "/api/v2/garage?connectivityGenerations=MOD1&connectivityGenerations=MOD2&connectivityGenerations=MOD3&connectivityGenerations=MOD4";
            return await GetAsync<Garage>(url, anonymize, Anonymizer.AnonymizeGarage);
        }
    }
}
