namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, FrozenDictionary<Date, string>> cache;

    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines if the date is a public holiday in Australian for all states.
        /// </summary>
        public bool IsNationalHoliday() =>
            ForNational(date.Year)
                .ContainsKey(date);

        /// <summary>
        ///  Determines if the date is a public holiday in Australian for all states.
        /// </summary>
        /// <param name="name">The name of the holiday.</param>
        public bool IsNationalHoliday([NotNullWhen(true)] out string? name) =>
            ForNational(date.Year)
                .TryGetValue(date, out name);
    }

    /// <summary>
    /// Gets all public holidays that exist in all states.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> ForNational(int year) =>
        cache.GetOrAdd(
            year,
            year =>
                BuildHolidays(year)
                    .ToFrozenDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildHolidays(int year)
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

        var anzacDate = AnzacDayCalculator.GetAnzacDay(year);
        yield return (anzacDate, "Anzac Day");
        if (anzacDate.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, April, 27), "Anzac Day (additional)");
        }

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}
