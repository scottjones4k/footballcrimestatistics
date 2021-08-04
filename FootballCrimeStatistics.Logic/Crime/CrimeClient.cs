using FootballCrimeStatistics.Logic.Crime.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.Crime
{
    public class CrimeClient : BaseHttpClient
    {
        public CrimeClient(HttpClient client, IDistributedCache cache) : base(client, cache)
        {

        }

        public async Task<IEnumerable<CrimeResponse>> GetCrimeForLocation(int year, int month, double latitude, double longitude) =>
            (await Get<List<CrimeResponse>>($"crimes-at-location?date={year}-{month}&lat={latitude}&lng={longitude}"));
    }
}
