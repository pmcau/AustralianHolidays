namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> waCache;

    /// <summary>
    ///  Determines if the date is a public holiday in Western Australia.
    ///  Reference: https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsWaHoliday(this Date date) =>
        GetWaHolidays(date.Year)
            .ContainsKey(date);

    /// <summary>
    ///  Gets all public holidays for  Western Australia.
    ///  Reference: https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia
    /// </summary>
    public static bool IsWaHoliday(this Date date, [NotNullWhen(true)] out string? name) =>
        GetWaHolidays(date.Year)
            .TryGetValue(date, out name);

    /// <summary>
    /// Gets all public holidays for Western Australia for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetWaHolidays(int year) =>
        waCache.GetOrAdd(
            year,
            year => BuildWaHolidays(year).ToDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildWaHolidays(int year)
    {
        yield return (new(year, (int) Month.January, 1), "New Year's Day");

        var australiaDay = GetAustraliaDay(year);
        if (australiaDay.IsWeekday())
        {
            yield return (australiaDay, "Australia Day");
        }
        else
        {
            if (australiaDay.DayOfWeek == DayOfWeek.Saturday)
            {
                yield return (new(year, (int) Month.January, 28), "Australia Day (additional)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, (int) Month.January, 27), "Australia Day (additional)");
            }
        }

        yield return (Extensions.GetFirstMonday(Month.March, year), "Labour Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        var anzacDate = AnzacDayCalculator.GetAnzacDay(year);
        yield return (anzacDate, "Anzac Day");
        if (anzacDate.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, (int) Month.April, 27), "Anzac Day (additional)");
        }

        if (anzacDate.DayOfWeek == DayOfWeek.Sunday)
        {
            yield return (new(year, (int) Month.April, 26), "Anzac Day (additional)");
        }

        yield return (Extensions.GetFirstMonday(Month.June, year), "Western Australia Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthdayWa(year);

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}