using FootballCrimeStatistics.Logic;
using FootballCrimeStatistics.Logic.Crime;
using FootballCrimeStatistics.Logic.Football;
using FootballCrimeStatistics.Logic.Football.Models;
using FootballCrimeStatistics.Logic.PostcodeLookup;
using FootballCrimeStatistics.Models.Response;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FootballCrimeStatistics.Logic.PostcodeLookup.Models;
using FootballCrimeStatistics.Logic.Crime.Models;
using Xunit;

namespace FootballCrimeStatistics.UnitTests
{
    public class FootballCrimeServiceTests
    {
        private readonly Mock<ICrimeService> _mockCrimeService;
        private readonly Mock<IPostcodeLookupService> _mockPostcodeService;
        private readonly Mock<IFootballService> _mockFootballService;
        private readonly FootballCrimeService _service;

        public FootballCrimeServiceTests()
        {
            _mockCrimeService = new Mock<ICrimeService>();
            _mockPostcodeService = new Mock<IPostcodeLookupService>();
            _mockFootballService = new Mock<IFootballService>();

            _service = new FootballCrimeService(_mockFootballService.Object, _mockPostcodeService.Object, _mockCrimeService.Object);
        }

        [Fact]
        public async Task GetCrimesByMonth_ReturnsAsExpected()
        {
            // Given
            StandardSetup();

            // When
            var result = await _service.GetCrimesByMonth(2021, 6);

            // Then
            var resA = result.FirstOrDefault(r => r.Name == "TeamA");
            var resB = result.FirstOrDefault(r => r.Name == "TeamB");

            AssertTeam(resA, "A", 42);
            AssertTeam(resB, "B", 43);
        }

        [Fact]
        public async Task GetCrimesForATeamByMonth_ReturnsAsExpected()
        {
            // Given
            StandardSetup();

            // When
            var result = await _service.GetCrimesForATeamByMonth(42, 2021, 6);

            // Then
            AssertTeam(result, "A", 42);
        }

        [Fact]
        public async Task GetCrimesByYear_ReturnsAsExpected()
        {
            // Given
            StandardSetup();

            // When
            var result = await _service.GetCrimesByYear(2021);

            // Then
            var resA = result.FirstOrDefault(r => r.Name == "TeamA");
            var resB = result.FirstOrDefault(r => r.Name == "TeamB");

            AssertTeam(resA, "A", 42, false);
            AssertTeam(resB, "B", 43);
            AssertCrime(resA.Crimes.First(), "A5", 5);
            AssertCrime(resA.Crimes.Last(), "A", 6);
        }

        [Fact]
        public async Task GetCrimesForATeamByYear_ReturnsAsExpected()
        {
            // Given
            StandardSetup();

            // When
            var result = await _service.GetCrimesForATeamByYear(42, 2021);

            // Then
            AssertTeam(result, "A", 42, false);
            AssertCrime(result.Crimes.First(), "A5", 5);
            AssertCrime(result.Crimes.Last(), "A", 6);
        }

        private void StandardSetup()
        {
            var teams = new List<TeamResponse> {
                new TeamResponse { Address = "TestAddressA", Id = 42, Name = "TeamA", ShortName = "A" },
                new TeamResponse { Address = "TestAddressB", Id = 43, Name = "TeamB", ShortName = "B" },
            };
            var addresses = new List<PostcodeResponse>
            {
                new PostcodeResponse { Postcode = "AddressA", Latitude = 53.777714, Longitude = -1.573104 },
                new PostcodeResponse { Postcode = "AddressB", Latitude = 53.777715, Longitude = -1.573105 }
            };
            var crimesA = new List<CrimeResponse>
            {
                new CrimeResponse { Category = "CatA", Context = "ContextA", Month = "2021-6",
                    Location = new LocationResponse { Street = new StreetResponse { Name = "StreetA" } },
                    OutcomeStatus = new OutcomeStatusResponse { Category = "OutcomeA" } }
            };
            var crimesA5 = new List<CrimeResponse>
            {
                new CrimeResponse { Category = "CatA5", Context = "ContextA5", Month = "2021-5",
                    Location = new LocationResponse { Street = new StreetResponse { Name = "StreetA5" } },
                    OutcomeStatus = new OutcomeStatusResponse { Category = "OutcomeA5" } }
            };
            var crimesB = new List<CrimeResponse>
            {
                new CrimeResponse { Category = "CatB", Context = "ContextB", Month = "2021-6",
                    Location = new LocationResponse { Street = new StreetResponse { Name = "StreetB" } },
                    OutcomeStatus = new OutcomeStatusResponse { Category = "OutcomeB" } }
            };
            _mockFootballService.Setup(s => s.GetTeams())
                .ReturnsAsync(teams);
            _mockFootballService.Setup(s => s.GetTeam(42))
                .ReturnsAsync(teams.First());
            _mockPostcodeService.Setup(s => s.GetLatLongs(It.Is<IEnumerable<string>>(e => e.Contains("TestAddressA") && e.Contains("TestAddressB") && e.Count() == 2)))
                .ReturnsAsync(addresses);
            _mockPostcodeService.Setup(s => s.GetLatLongs(It.Is<IEnumerable<string>>(e => e.Contains("TestAddressA") && !e.Contains("TestAddressB") && e.Count() == 1)))
                .ReturnsAsync(addresses.Take(1));
            _mockCrimeService.Setup(s => s.GetCrimeForLocation(2021, It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(new List<CrimeResponse>());
            _mockCrimeService.Setup(s => s.GetCrimeForLocation(2021, 6, addresses.First().Latitude, addresses.First().Longitude))
                .ReturnsAsync(crimesA);
            _mockCrimeService.Setup(s => s.GetCrimeForLocation(2021, 5, addresses.First().Latitude, addresses.First().Longitude))
                .ReturnsAsync(crimesA5);
            _mockCrimeService.Setup(s => s.GetCrimeForLocation(2021, 6, addresses.Last().Latitude, addresses.Last().Longitude))
                .ReturnsAsync(crimesB);
        }

        private static void AssertTeam(Team result, string suffix, int id, bool assertCrime = true)
        {
            Assert.Equal(id, result.Id);
            Assert.Equal(suffix, result.ShortName);
            Assert.Equal($"TestAddress{suffix}", result.Address);
            if (assertCrime)
            {
                Assert.Single(result.Crimes);
                AssertCrime(result.Crimes.First(), suffix);
            }
        }

        private static void AssertCrime(Crime result, string suffix, int month = 6)
        {
            Assert.Equal($"Cat{suffix}", result.Category);
            Assert.Equal($"Context{suffix}", result.Context);
            Assert.Equal($"2021-{month}", result.Month);
            Assert.Equal($"Outcome{suffix}", result.Outcome);
            Assert.Equal($"Street{suffix}", result.StreetName);
        }
    }
}
