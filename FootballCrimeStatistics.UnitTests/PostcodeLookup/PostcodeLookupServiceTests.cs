using FootballCrimeStatistics.Logic.PostcodeLookup;
using FootballCrimeStatistics.UnitTests.Mocks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballCrimeStatistics.UnitTests.PostcodeLookup
{
    public class PostcodeLookupServiceTests
    {
        private readonly MockHttpClient _mockClient;
        private readonly IDistributedCache _cache;
        private readonly PostcodeLookupService _service;

        public PostcodeLookupServiceTests()
        {
            _mockClient = new MockHttpClient();
            _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            var postcodeClient = new PostcodeClient(_mockClient.Client, _cache);
            _service = new PostcodeLookupService(postcodeClient);
        }

        [Theory]
        [MemberData(nameof(Addresses))]
        public void PostcodeFoundCorrectly(string address, string postcode)
        {
            Assert.Equal(postcode, PostcodeLookupService.ParsePostcodeFromAddressString(address));
        }


        public static IEnumerable<object[]> Addresses =>
        new List<object[]>
        {
            new object[] { "Elland Road Leeds LS11 0ES", "LS11 0ES" },
            new object[] { "75 Drayton Park London N5 1BU", "N5 1BU" },
            new object[] { "Villa Park Birmingham B6 6HE", "B6 6HE" },
            new object[] { "Fulham Road London SW6 1HS", "SW6 1HS" },
            new object[] { "Goodison Park Liverpool L4 4EL", "L4 4EL" },
        };

        [Fact]
        public async Task GetLatLongs_ReturnsAsExpected()
        {
            // Given
            _mockClient.SetReturn("/postcodes", "{\"status\":200,\"result\":[{\"query\":\"LS11 0ES\",\"result\":{\"postcode\":\"LS11 0ES\",\"longitude\":-1.573104,\"latitude\":53.777714}}]}");

            // When
            var result = await _service.GetLatLongs(new List<string> { "LS11 0ES" });

            // Then
            Assert.Single(result);
            Assert.Equal(-1.573104, result.First().Longitude);
        }
    }
}
