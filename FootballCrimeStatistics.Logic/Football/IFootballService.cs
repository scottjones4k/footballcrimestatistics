using FootballCrimeStatistics.Logic.Football.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.Football
{
    public interface IFootballService
    {
        Task<IEnumerable<TeamResponse>> GetTeams();
        Task<TeamResponse> GetTeam(int teamId);
    }
}