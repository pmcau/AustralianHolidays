namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> vicHolidays = new();

    /// <summary>
    ///  Determines if the date is a public holiday in Victoria.
    ///  Reference: https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2025
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsVicHoliday(this Date date) =>
        IsVicHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in Victoria.
    ///  Reference: https://business.vic.gov.au/business-information/public-holidays/victorian-public-holidays-2025
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsVicHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetVicHolidays(date.Year);

        return holidays.TryGetValue(date, out name);
    }

    /// <summary>
    /// Gets all public holidays for Victoria for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetVicHolidays(int year) =>
        vicHolidays.GetOrAdd(
            year,
            year => BuildVicHolidays(year).ToDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildVicHolidays(int year)
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

        yield return (Extensions.GetSecondMonday(Month.March, year), "Labour Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (AnzacDayCalculator.GetAnzacDay(year), "Anzac Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetLastFriday(Month.September, year), "Friday before AFL Grand Final (Subject to AFL schedule)");
        yield return (Extensions.GetFirstTuesday(Month.November, year), "Melbourne Cup Day");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}