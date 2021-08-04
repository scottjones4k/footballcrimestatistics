using FootballCrimeStatistics.Logic.Crime.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.Crime
{
    public class CrimeService : ICrimeService
    {
        private readonly CrimeClient _crimeClient;

        public CrimeService(CrimeClient crimeClient)
        {
            _crimeClient = crimeClient;
        }

        public async Task<IEnumerable<CrimeResponse>> GetCrimeForLocation(int year, int month, double latitude, double longitude) =>
            await _crimeClient.GetCrimeForLocation(year, month, latitude, longitude);
    }
}
