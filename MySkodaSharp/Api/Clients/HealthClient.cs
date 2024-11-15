using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class HealthClient : BaseClient
    {
        public HealthClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Health>> GetHealthAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v1/vehicle-health-report/warning-lights/{vin}";
            return await GetAsync<Health>(url, anonymize, Anonymizer.AnonymizeHealth);
        }
    }
}
