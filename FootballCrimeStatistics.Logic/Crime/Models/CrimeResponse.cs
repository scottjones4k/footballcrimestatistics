using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.Crime.Models
{
    public record CrimeResponse
    {
        [JsonPropertyName("category")]
        public string Category { get; init; }

        [JsonPropertyName("context")]
        public string Context { get; init; }

        [JsonPropertyName("month")]
        public string Month { get; init; }

        [JsonPropertyName("outcome_status")]
        public OutcomeStatusResponse OutcomeStatus { get; init; }

        [JsonPropertyName("location")]
        public LocationResponse Location { get; init; }
    }
}
