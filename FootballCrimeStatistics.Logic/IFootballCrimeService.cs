using FootballCrimeStatistics.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic
{
    public interface IFootballCrimeService
    {
        Task<IEnumerable<Team>> GetCrimesByMonth(int year, int month);
        Task<IEnumerable<Team>> GetCrimesByYear(int year);
        Task<Team> GetCrimesForATeamByMonth(int teamId, int year, int month);
        Task<Team> GetCrimesForATeamByYear(int teamId, int year);
    }
}