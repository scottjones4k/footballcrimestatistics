using System.Collections.Generic;

namespace FootballCrimeStatistics.Models.Response
{
    public record Team(int Id, string Name, string ShortName, string Address, IEnumerable<Crime> Crimes);
}
