using FootballCrimeStatistics.Logic.Crime;
using FootballCrimeStatistics.Logic.Football;
using FootballCrimeStatistics.Logic.Football.Models;
using FootballCrimeStatistics.Logic.PostcodeLookup;
using FootballCrimeStatistics.Models.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic
{
    public class FootballCrimeService : IFootballCrimeService
    {
        private readonly IFootballService _footballService;
        private readonly IPostcodeLookupService _postcodeService;
        private readonly ICrimeService _crimeService;

        public FootballCrimeService(IFootballService footballService, IPostcodeLookupService postcodeService, ICrimeService crimeService)
        {
            _footballService = footballService;
            _postcodeService = postcodeService;
            _crimeService = crimeService;
        }

        public async Task<IEnumerable<Team>> GetCrimesByMonth(int year, int month)
        {
            var teams = await _footballService.GetTeams();
            return await GetForTeamsCrimesByMonth(teams, year, month).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetCrimesByYear(int year)
        {
            var teams = await _footballService.GetTeams();
            return await GetCrimesForTeamsByYear(teams, year);
        }

        public async Task<Team> GetCrimesForATeamByMonth(int teamId, int year, int month)
        {
            var team = await _footballService.GetTeam(teamId);
            return (await GetForTeamsCrimesByMonth(new List<TeamResponse> { team }, year, month).ToListAsync()).First();
        }

        public async Task<Team> GetCrimesForATeamByYear(int teamId, int year)
        {
            var team = await _footballService.GetTeam(teamId);
            return (await GetCrimesForTeamsByYear(new List<TeamResponse> { team }, year)).First();
        }

        private async Task<IEnumerable<Team>> GetCrimesForTeamsByYear(IEnumerable<TeamResponse> teams, int year)
        {
            var crimeList = new List<Team>();
            foreach (var month in Enumerable.Range(1, 12))
            {
                crimeList.AddRange(await GetForTeamsCrimesByMonth(teams, year, month).ToListAsync());
            }

            // Merge all crimes per team together into one Team model
            return crimeList.GroupBy(t => t.Id).Select(t => t.First() with { Crimes = t.SelectMany(s => s.Crimes).OrderBy(s => s.Month) });
        }

        private async IAsyncEnumerable<Team> GetForTeamsCrimesByMonth(IEnumerable<TeamResponse> teams, int year, int month)
        {
            var latlongs = await _postcodeService.GetLatLongs(teams.Select(e => e.Address));
            foreach (var latlong in latlongs)
            {
                var team = teams.FirstOrDefault(t => t.Address.EndsWith(latlong.Postcode, System.StringComparison.InvariantCultureIgnoreCase));
                // Must call service one by one to avoid getting error code 429 returned
                var crimes = await _crimeService.GetCrimeForLocation(year, month, latlong.Latitude, latlong.Longitude);
                var mappedCrimes = crimes.Select(c => new Models.Response.Crime(c.Category, c.Context, c.OutcomeStatus?.Category, c.Month, c.Location?.Street?.Name));
                yield return new Team(team.Id, team.Name, team.ShortName, team.Address, mappedCrimes);
            }
        }
    }
}
