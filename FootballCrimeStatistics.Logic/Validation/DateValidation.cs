using System;

namespace FootballCrimeStatistics.Logic.Validation
{
    public static class DateValidation
    {
        public static bool IsValidYear(int year) =>
            year >= 2000 && year <= DateTime.Now.Year;

        public static bool IsValidMonth(int year, int month) =>
            month >= 1 && ((DateTime.Now.Year > year && month <= 12) || month < DateTime.Now.Month);
    }
}
