namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> cache;

    /// <summary>
    ///  Determines if the date is a public holiday in Australian for all states.
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsHoliday(this Date date) =>
        GetHolidays(date.Year)
            .ContainsKey(date);

    /// <summary>
    ///  Determines if the date is a public holiday in Australian for all states.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsHoliday(this Date date, [NotNullWhen(true)] out string? name) =>
        GetHolidays(date.Year)
            .TryGetValue(date, out name);

    /// <summary>
    /// Gets all public holidays that exist in all states.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetHolidays(int year) =>
        cache.GetOrAdd(
            year,
            year => BuildHolidays(year)
                .ToDictionary(_ => _.date, _ => _.name));

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
                yield return (new(year, January, 28), "Australia Day (additional)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, January, 27), "Australia Day (additional)");
            }
        }

        var anzacDate = AnzacDayCalculator.GetAnzacDay(year);

        if (anzacDate.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, April, 27), "Anzac Day (additional)");
        }
        else
        {
            yield return (anzacDate, "Anzac Day");
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