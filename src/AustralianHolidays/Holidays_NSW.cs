namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    ///  Determines if the date is a public holiday in the New South Wales.
    ///  Reference: https://www.nsw.gov.au/about-nsw/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsNswHoliday(this Date date) =>
        IsNswHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in the New South Wales.
    ///  Reference: https://www.nsw.gov.au/about-nsw/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsNswHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetNswHolidays(date.Year);
        name = holidays
            .Where(_ => _.date == date)
            .Select(_ => _.name)
            .SingleOrDefault();
        return name != null;
    }

    /// <summary>
    ///  Gets all public holidays for New South Wales.
    ///  Reference: https://www.nsw.gov.au/about-nsw/public-holidays
    /// </summary>
    public static IEnumerable<(Date date, string name)> GetNswHolidays(int year)
    {
        var newYears = new Date(year, (int) Month.January, 1);
        yield return (newYears, "New Year's Day");

        if (newYears.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, (int) Month.January, 3), "New Year's Day (additional)");
        }

        if (newYears.DayOfWeek == DayOfWeek.Sunday)
        {
            yield return (new(year, (int) Month.January, 2), "New Year's Day (additional)");
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
                yield return (new(year, (int) Month.January, 28), "Australia Day (additional)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, (int) Month.January, 27), "Australia Day (additional)");
            }
        }

        yield return (new(year, (int) Month.April, 25), "Anzac Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(Month.August, year), "Bank Holiday");

        yield return (Extensions.GetFirstMonday(Month.October, year), "Labour Day");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}