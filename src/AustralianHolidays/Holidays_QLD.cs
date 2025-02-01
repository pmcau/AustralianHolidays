namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> qldCache;

    /// <summary>
    ///  Determines if the date is a public holiday in Queensland.
    ///  Reference: https://www.qld.gov.au/recreation/travel/holidays/public
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsQldHoliday(this Date date) =>
        GetQldHolidays(date.Year)
            .ContainsKey(date);

    /// <summary>
    ///  Determines if the date is a public holiday in Queensland.
    ///  Reference: https://www.qld.gov.au/recreation/travel/holidays/public
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsQldHoliday(this Date date, [NotNullWhen(true)] out string? name) =>
        GetQldHolidays(date.Year)
            .TryGetValue(date, out name);

    /// <summary>
    ///  Gets all public holidays for Queensland for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetQldHolidays(int year) =>
        qldCache.GetOrAdd(
            year,
            year => BuildQldHolidays(year).ToDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildQldHolidays(int year)
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

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (AnzacDayCalculator.GetAnzacDay(year), "Anzac Day");

        yield return (Extensions.GetFirstMonday(May, year), "Labour Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthdayQld(year);

        yield return (ChristmasCalculator.ChristmasEve(year), "Christmas Eve (partial day)");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}