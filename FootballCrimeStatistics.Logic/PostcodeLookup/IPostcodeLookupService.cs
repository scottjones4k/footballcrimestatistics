using FootballCrimeStatistics.Logic.PostcodeLookup.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.PostcodeLookup
{
    public interface IPostcodeLookupService
    {
        Task<IEnumerable<PostcodeResponse>> GetLatLongs(IEnumerable<string> addresses);
    }
}