using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.Football.Models
{
    public record CompetitionResponse
    {
        [JsonPropertyName("teams")]
        public IEnumerable<TeamResponse> Teams { get; init; }
    }
}
