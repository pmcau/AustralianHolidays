namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    ///  Determines if the date is a public holiday in the Northern Territory.
    ///  Reference: https://nt.gov.au/nt-public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsNtHoliday(this Date date) =>
        IsNtHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in the Northern Territory.
    ///  Reference: https://nt.gov.au/nt-public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsNtHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetNtHolidays(date.Year);
        name = holidays
            .Where(_ => _.date == date)
            .Select(_ => _.name)
            .SingleOrDefault();
        return name != null;
    }

    /// <summary>
    ///  Gets all public holidays for the Northern Territory.
    ///  Reference: https://nt.gov.au/nt-public-holidays
    /// </summary>
    public static IEnumerable<(Date date, string name)> GetNtHolidays(int year)
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

        yield return (Extensions.GetFirstMonday(Month.May, year), "May Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (new(year, (int) Month.April, 25), "Anzac Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(Month.August, year), "Picnic Day");

        yield return (ChristmasCalculator.ChristmasEve(year), "Christmas Eve (partial day)");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }

        yield return (new(year, (int) Month.December, 31), "New Year's Eve (partial day)");
    }
}