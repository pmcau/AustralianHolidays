namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> nswCache;

    /// <summary>
    ///  Determines if the date is a public holiday in the New South Wales.
    ///  Reference: https://www.nsw.gov.au/about-nsw/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsNswHoliday(this Date date) =>
        GetNswHolidays(date.Year)
            .ContainsKey(date);

    /// <summary>
    ///  Determines if the date is a public holiday in the New South Wales.
    ///  Reference: https://www.nsw.gov.au/about-nsw/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsNswHoliday(this Date date, [NotNullWhen(true)] out string? name) =>
        GetNswHolidays(date.Year)
            .TryGetValue(date, out name);

    /// <summary>
    ///  Gets all public holidays for New South Wales for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetNswHolidays(int year) =>
        nswCache.GetOrAdd(
            year,
            year => BuildNswHolidays(year).ToDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildNswHolidays(int year)
    {
        var newYears = new Date(year, January, 1);
        yield return (newYears, "New Year's Day");

        if (newYears.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, January, 3), "New Year's Day (additional)");
        }

        if (newYears.DayOfWeek == DayOfWeek.Sunday)
        {
            yield return (new(year, January, 2), "New Year's Day (additional)");
        }

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

        yield return (AnzacDayCalculator.GetAnzacDay(year), "Anzac Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(August, year), "Bank Holiday");

        yield return (Extensions.GetFirstMonday(October, year), "Labour Day");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}