using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class MaintenanceClient : BaseClient
    {
        public MaintenanceClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Maintenance>> GetMaintenanceAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v3/vehicle-maintenance/vehicles/{vin}";
            return await GetAsync<Maintenance>(url, anonymize, Anonymizer.AnonymizeMaintenance);
        }
    }
}
