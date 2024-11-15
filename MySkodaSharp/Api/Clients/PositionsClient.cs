using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class PositionsClient : BaseClient
    {
        public PositionsClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<Positions>> GetPositionsAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v1/maps/positions?vin={vin}";
            return await GetAsync<Positions>(url, anonymize, Anonymizer.AnonymizePositions);
        }
    }
}
