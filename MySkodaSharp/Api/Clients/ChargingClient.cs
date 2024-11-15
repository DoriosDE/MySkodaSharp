using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class ChargingClient : BaseClient
    {
        public ChargingClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Charging>> GetChargingAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v1/charging/{vin}";
            return await GetAsync<Charging>(url, anonymize, Anonymizer.AnonymizeCharging);
        }
    }
}
