using FootballCrimeStatistics.Logic.Football.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.Football
{
    public class FootballService : IFootballService
    {
        private readonly FootballClient _footballClient;

        public FootballService(FootballClient footballClient)
        {
            _footballClient = footballClient;
        }

        public Task<IEnumerable<TeamResponse>> GetTeams() =>
            _footballClient.GetTeams(2021);

        public Task<TeamResponse> GetTeam(int teamId) =>
            _footballClient.GetTeam(teamId);
    }
}
