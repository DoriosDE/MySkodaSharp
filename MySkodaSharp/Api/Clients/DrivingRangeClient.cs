using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class DrivingRangeClient : BaseClient
    {
        public DrivingRangeClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<DrivingRange>> GetDrivingRangeAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v2/vehicle-status/{vin}/driving-range";
            return await GetAsync<DrivingRange>(url, anonymize, Anonymizer.AnonymizeDrivingRange);
        }
    }
}
