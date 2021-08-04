using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.Crime.Models
{
    public record LocationResponse
    {
        [JsonPropertyName("street")]
        public StreetResponse Street { get; init; }
    }
}
