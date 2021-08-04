using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.Crime.Models
{
    public record StreetResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }
    }
}
