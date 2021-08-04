using System.Text.Json.Serialization;

namespace FootballCrimeStatistics.Logic.PostcodeLookup.Models
{
    public record Response<T>
    {
        [JsonPropertyName("status")]
        public int Status { get; init; }

        [JsonPropertyName("result")]
        public T Result { get; init; }
    }
}
