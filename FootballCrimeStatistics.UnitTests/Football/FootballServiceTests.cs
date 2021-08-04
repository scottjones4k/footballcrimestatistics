using FootballCrimeStatistics.Logic.Football;
using FootballCrimeStatistics.UnitTests.Mocks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballCrimeStatistics.UnitTests.Football
{
    public class FootballServiceTests
    {
        private readonly MockHttpClient _mockClient;
        private readonly FootballService _service;
        private readonly IDistributedCache _cache;

        public FootballServiceTests()
        {
            _mockClient = new MockHttpClient();
            _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            var client = new FootballClient(_mockClient.Client, _cache);
            _service = new FootballService(client);
        }

        [Fact]
        public async Task GetTeams_ReturnsAsExpected()
        {
            // Given
            _mockClient.SetReturn("/competitions/2021/teams", "{\"teams\":[{\"id\":38, \"address\": \"some place\"}, {\"id\":74, \"address\": \"some other place\"}]}");

            // When
            var result = await _service.GetTeams();

            // Then
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetTeam_ReturnsAsExpected()
        {
            // Given
            _mockClient.SetReturn("/teams/38", "{\"id\":38, \"address\": \"some place\"}");

            // When
            var result = await _service.GetTeam(38);

            // Then
            Assert.Equal(38, result.Id);
        }
    }
}
