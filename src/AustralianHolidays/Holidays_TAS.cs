namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> tasHolidays = new();

    /// <summary>
    ///  Determines if the date is a public holiday in Tasmania.
    ///  Reference: https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsTasHoliday(this Date date) =>
        IsTasHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in Tasmania.
    ///  Reference: https://worksafe.tas.gov.au/topics/laws-and-compliance/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsTasHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetTasHolidays(date.Year);

        return holidays.TryGetValue(date, out name);
    }

    /// <summary>
    ///  Gets all public holidays for Tasmania for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetTasHolidays(int year) =>
        tasHolidays.GetOrAdd(
            year,
            year => BuildTasHolidays(year).ToDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildTasHolidays(int year)
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

        yield return (Extensions.GetSecondMonday(Month.March, year), "Eight Hours Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
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