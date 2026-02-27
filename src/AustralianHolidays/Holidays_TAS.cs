namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, FrozenDictionary<Date, string>> tasCache;

    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines if the date is a public holiday in Tasmania.
        ///  Reference: https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays
        /// </summary>
        public bool IsTasHoliday() =>
            ForTas(date.Year)
                .ContainsKey(date);

        /// <summary>
        ///  Determines if the date is a public holiday in Tasmania.
        ///  Reference: https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays
        /// </summary>
        /// <param name="name">The name of the holiday.</param>
        public bool IsTasHoliday([NotNullWhen(true)] out string? name) =>
            ForTas(date.Year)
                .TryGetValue(date, out name);
    }

    /// <summary>
    ///  Gets all public holidays for Tasmania for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> ForTas(int year) =>
        tasCache.GetOrAdd(
            year,
            year =>
                BuildTasHolidays(year)
                    .ToFrozenDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildTasHolidays(int year)
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

        yield return (Extensions.GetSecondMonday(March, year), "Eight Hours Day");

        var (easterFriday, _, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (easterMonday.AddDays(1), "Easter Tuesday (Government employees only)");

        yield return (AnzacDayCalculator.GetAnzacDay(year), "Anzac Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}
