using FootballCrimeStatistics.Logic.Football.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic.Football
{
    public class FootballClient : BaseHttpClient
    {
        public FootballClient(HttpClient client, IDistributedCache cache) : base(client, cache)
        {
        }

        public async Task<IEnumerable<TeamResponse>> GetTeams(int competitionId) =>
            (await Get<CompetitionResponse>($"competitions/{competitionId}/teams")).Teams;

        public async Task<TeamResponse> GetTeam(int teamId) =>
            await Get<TeamResponse>($"teams/{teamId}");
    }
}
