using MySkodaSharp.Api.Models;
using MySkodaSharp.Api.Utils;
using MySkodaSharp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySkodaSharp.Api.Clients
{
    internal class TripStatisticsClient : BaseClient
    {
        public TripStatisticsClient(HttpClient client)
            : base(client) { }

        public async Task<GetEndpointResult<TripStatistics>> GetTripStatisticsAsync(string vin, bool anonymize = false)
        {
            var url = $"/api/v1/trip-statistics/{vin}?offsetType=week&offset=0&timezone=Europe%2FBerlin";
            return await GetAsync<TripStatistics>(url, anonymize, Anonymizer.AnonymizeTripStatistics);
        }
    }
}
