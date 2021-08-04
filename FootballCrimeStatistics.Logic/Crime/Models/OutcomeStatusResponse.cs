using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.Crime.Models
{
    public record OutcomeStatusResponse
    {
        [JsonPropertyName("category")]
        public string Category { get; init; }
    }
}
