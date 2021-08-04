using FootballCrimeStatistics.Logic.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FootballCrimeStatistics.UnitTests.Validation
{
    public class DateValidationTests
    {
        [Theory]
        [MemberData(nameof(Years))]

        public void IsValidYear_ReturnsAsExpected(int year, bool expected)
        {
            Assert.Equal(expected, DateValidation.IsValidYear(year));
        }

        public static IEnumerable<object[]> Years =>
        new List<object[]>
        {
            new object[] { 1999, false },
            new object[] { 2000, true },
            new object[] { 2021, true },
            new object[] { 2022, false }
        };

        [Theory]
        [MemberData(nameof(YearsAndMonths))]

        public void IsValidMonth_ReturnsAsExpected(int year, int month, bool expected)
        {
            Assert.Equal(expected, DateValidation.IsValidMonth(year, month));
        }

        public static IEnumerable<object[]> YearsAndMonths =>
        new List<object[]>
        {
            new object[] { 2020, 0, false },
            new object[] { 2020, 13, false },
            new object[] { 2020, 1, true },
            new object[] { 2020, 12, true },
            new object[] { 2021, 0, false },
            new object[] { 2021, 13, false },
            new object[] { 2021, 1, true },
            new object[] { 2021, DateTime.Now.Month-1, true },
            new object[] { 2021, DateTime.Now.Month, false }
        };
    }
}
