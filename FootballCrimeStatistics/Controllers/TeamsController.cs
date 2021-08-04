using FootballCrimeStatistics.Logic;
using FootballCrimeStatistics.Logic.Crime.Models;
using FootballCrimeStatistics.Logic.Validation;
using FootballCrimeStatistics.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly IFootballCrimeService _footballCrimeService;

        public TeamsController(IFootballCrimeService footballCrimeService)
        {
            _footballCrimeService = footballCrimeService;
        }

        [HttpGet("{teamId}/Crimes/{year}/{month}")]
        [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCrimes(int teamId, int year, int month)
        {
            if (!DateValidation.IsValidYear(year)) return BadRequest("Year is invalid");
            if (!DateValidation.IsValidMonth(year, month)) return BadRequest("Month must be a valid month in the past");

            return Ok(await _footballCrimeService.GetCrimesForATeamByMonth(teamId, year, month));
        }

        [HttpGet("{teamId}/Crimes/{year}")]
        [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByYear(int teamId, int year)
        {
            if (!DateValidation.IsValidYear(year)) return BadRequest("Year is invalid");

            return Ok(await _footballCrimeService.GetCrimesForATeamByYear(teamId, year));
        }
    }
}
