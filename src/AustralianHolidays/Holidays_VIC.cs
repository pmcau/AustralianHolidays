namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, FrozenDictionary<Date, string>> vicCache;

    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines if the date is a public holiday in Victoria.
        ///  Reference: https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2026
        /// </summary>
        public bool IsVicHoliday() =>
            ForVic(date.Year)
                .ContainsKey(date);

        /// <summary>
        ///  Determines if the date is a public holiday in Victoria.
        ///  Reference: https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2026
        /// </summary>
        /// <param name="name">The name of the holiday.</param>
        public bool IsVicHoliday([NotNullWhen(true)] out string? name) =>
            ForVic(date.Year)
                .TryGetValue(date, out name);
    }

    /// <summary>
    /// Gets all public holidays for Victoria for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> ForVic(int year) =>
        vicCache.GetOrAdd(
            year,
            year =>
                BuildVicHolidays(year)
                    .ToFrozenDictionary(_ => _.date, _ => _.name));

    static (Date date, string name) GetAflFinalFriday(int year)
    {
        // Actual historical dates for AFL Final Friday holiday
        // Source: https://publicholidays.com.au/afl-grand-final-holiday/
        var historicalDates = new Dictionary<int, (int month, int day)>
        {
            { 2015, (10, 2) },   // Final: Oct 3, 2015 (first Saturday in October)
            { 2016, (9, 30) },   // Final: Oct 1, 2016
            { 2017, (9, 29) },   // Final: Sep 30, 2017
            { 2018, (9, 28) },   // Final: Sep 29, 2018
            { 2019, (9, 27) },   // Final: Sep 28, 2019
            { 2020, (10, 23) },  // Final: Oct 24, 2020 (COVID - played in Brisbane)
            { 2021, (9, 24) },   // Final: Sep 25, 2021 (COVID - played in Perth)
            { 2022, (9, 23) },   // Final: Sep 24, 2022
            { 2023, (9, 29) },   // Final: Sep 30, 2023
            { 2024, (9, 27) },   // Final: Sep 28, 2024
            { 2025, (9, 26) }    // Final: Sep 26, 2025

            // TODO: 2026 - Friday before AFL Grand Final typically falls on the last Friday in September
            // awaiting exact date upon release of the 2026 AFL schedule.
        };

        if (historicalDates.TryGetValue(year, out var dateInfo))
        {
            // Use actual historical date (no disclaimer needed)
            return (new(year, dateInfo.month, dateInfo.day), "Friday before AFL Final");
        }

        // For future years, use heuristic (last Friday of September) with disclaimer
        var lastFridayOfSeptember = Extensions.GetLastFriday(September, year);
        return (lastFridayOfSeptember, "Friday before AFL Final (Subject to AFL schedule)");
    }

    static IEnumerable<(Date date, string name)> BuildVicHolidays(int year)
    {
        yield return (new(year, January, 1), "New Year's Day");

        var australiaDay = GetAustraliaDay(year);
        if (australiaDay.IsWeekday())
        {
            yield return (australiaDay, "Australia Day");
        }
        else
        {
            if (australiaDay.DayOfWeek == DayOfWeek.Saturday)
            {
                yield return (new(year, January, 28), "Australia Day (observed)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, January, 27), "Australia Day (observed)");
            }
        }

        yield return (Extensions.GetSecondMonday(March, year), "Labour Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (AnzacDayCalculator.GetAnzacDay(year), "Anzac Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        // AFL Final Friday - use actual historical dates where known
        var (aflFriday, aflName) = GetAflFinalFriday(year);
        yield return (aflFriday, aflName);
        yield return (Extensions.GetFirstTuesday(November, year), "Melbourne Cup Day");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}
