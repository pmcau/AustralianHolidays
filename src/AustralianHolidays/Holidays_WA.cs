namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    ///  Determines if the date is a public holiday in Western Australia.
    ///  Reference: https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsWaHoliday(this Date date) =>
        IsWaHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in Western Australia.
    ///  Reference: https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsWaHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetWaHolidays(date.Year);
        name = holidays
            .Where(_ => _.date == date)
            .Select(_ => _.name)
            .SingleOrDefault();
        return name != null;
    }

    /// <summary>
    ///  Determines if the date is a public holiday in Western Australia.
    ///  Reference: https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static IEnumerable<(Date date, string name)> GetWaHolidays(int year)
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

        var anzacDate = new Date(year, (int) Month.April, 25);
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