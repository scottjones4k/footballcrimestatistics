using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.Football.Models
{
    public record TeamResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; init; }

        [JsonPropertyName("address")]
        public string Address { get; init; }
    }
}
