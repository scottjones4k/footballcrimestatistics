using FootballCrimeStatistics.Logic.PostcodeLookup.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FootballCrimeStatistics.UnitTests")]
namespace FootballCrimeStatistics.Logic.PostcodeLookup
{
    public class PostcodeLookupService : IPostcodeLookupService
    {
        private readonly PostcodeClient _postcodeClient;
        private static readonly Regex PostcodeLookup = new (@"(([A-Z][A-HJ-Y]?\d[A-Z\d]? ?\d[A-Z]{2}|GIR ?0A{2}))$", RegexOptions.Compiled);

        public PostcodeLookupService(PostcodeClient postcodeClient)
        {
            _postcodeClient = postcodeClient;
        }

        public async Task<IEnumerable<PostcodeResponse>> GetLatLongs(IEnumerable<string> addresses)
        {
            var postcodes = addresses.Select(a => ParsePostcodeFromAddressString(a));
            return await _postcodeClient.GetLatLongs(postcodes);
        }

        internal static string ParsePostcodeFromAddressString(string address)
        {
            var match = PostcodeLookup.Match(address);
            return match.Value;
        }
    }
}
