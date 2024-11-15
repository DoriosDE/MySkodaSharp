using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class StatusClient : BaseClient
    {
        public StatusClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Status>> GetStatusAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v2/vehicle-status/{vin}";
            return await GetAsync<Status>(url, anonymize, Anonymizer.AnonymizeStatus);
        }
    }
}
