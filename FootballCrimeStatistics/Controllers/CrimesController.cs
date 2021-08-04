using FootballCrimeStatistics.Logic;
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
    public class CrimesController : ControllerBase
    {
        private readonly IFootballCrimeService _footballCrimeService;

        public CrimesController(IFootballCrimeService footballCrimeService)
        {
            _footballCrimeService = footballCrimeService;
        }

        [HttpGet("{year}/{month}")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByMonth(int year, int month)
        {
            if (!DateValidation.IsValidYear(year)) return BadRequest("Year is invalid");
            if (!DateValidation.IsValidMonth(year, month)) return BadRequest("Month must be a valid month in the past");

            return Ok(await _footballCrimeService.GetCrimesByMonth(year, month));
        }

        [HttpGet("{year}")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByYear(int year)
        {
            if (!DateValidation.IsValidYear(year)) return BadRequest("Year is invalid");

            return Ok(await _footballCrimeService.GetCrimesByYear(year));
        }
    }
}
