using FootballCrimeStatistics.Logic.PostcodeLookup.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.PostcodeLookup
{
    public class PostcodeClient : BaseHttpClient
    {
        public PostcodeClient(HttpClient client, IDistributedCache cache) : base(client, cache)
        {
        }

        public async Task<IEnumerable<PostcodeResponse>> GetLatLongs(IEnumerable<string> postcodes) =>
            (await Post<Response<IEnumerable<BulkPostcodeResponse>>>($"postcodes", new { postcodes }, true)).Result.Select(r => r.Result);
    }
}
