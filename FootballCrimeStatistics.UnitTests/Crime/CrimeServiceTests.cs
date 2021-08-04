using FootballCrimeStatistics.Logic.Crime;
using FootballCrimeStatistics.UnitTests.Mocks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CrimeCrimeStatistics.UnitTests.Crime
{
    public class CrimeServiceTests
    {
        private readonly MockHttpClient _mockClient;
        private readonly CrimeService _service;
        private readonly IDistributedCache _cache;

        public CrimeServiceTests()
        {
            _mockClient = new MockHttpClient();
            _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            var client = new CrimeClient(_mockClient.Client, _cache);
            _service = new CrimeService(client);
        }

        [Fact]
        public async Task GetCrimeForLocation_ReturnsAsExpected()
        {
            // Given
            var year = 2021;
            var month = 6;
            var latitude = 53.777714;
            var longitude = -1.573104;
            _mockClient.SetReturn($"/crimes-at-location?date={year}-{month}&lat={latitude}&lng={longitude}", "[{\"category\": \"violent-crime\", \"context\": \"\",\"outcome_status\": {\"category\": \"Unable to prosecute suspect\"}}, {\"category\": \"burglary\", \"context\": \"\",\"outcome_status\": {\"category\": \"Offender fined\"}}]");

            // When
            var result = await _service.GetCrimeForLocation(year, month, latitude, longitude);

            // Then
            Assert.Equal(2, result.Count());
            Assert.Equal("violent-crime", result.First().Category);
        }
    }
}
