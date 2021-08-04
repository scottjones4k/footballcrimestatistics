using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.PostcodeLookup.Models
{
    public record PostcodeResponse
    {
        [JsonPropertyName("postcode")]
        public string Postcode { get; init; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; init; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; init; }
    }
}
