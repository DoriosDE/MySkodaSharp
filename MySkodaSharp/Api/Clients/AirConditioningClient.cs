using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class AirConditioningClient : BaseClient
    {
        public AirConditioningClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<AirConditioning>> GetAirConditioningAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v2/air-conditioning/{vin}";
            return await GetAsync<AirConditioning>(url, anonymize, Anonymizer.AnonymizeAirConditioning);
        }
    }
}
