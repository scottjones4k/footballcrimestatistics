using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.PostcodeLookup.Models
{
    public record BulkPostcodeResponse
    {
        [JsonPropertyName("query")]
        public string Query { get; init; }

        [JsonPropertyName("result")]
        public PostcodeResponse Result { get; init; }
    }
}
