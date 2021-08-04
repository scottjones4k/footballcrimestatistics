using FootballCrimeStatistics.Logic.Crime.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.Crime
{
    public interface ICrimeService
    {
        Task<IEnumerable<CrimeResponse>> GetCrimeForLocation(int year, int month, double latitude, double longitude);
    }
}