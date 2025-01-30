namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    ///  Determines if the date is a public holiday in South Australia.
    ///  Reference: https://www.safework.sa.gov.au/resources/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsSaHoliday(this Date date) =>
        IsSaHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in South Australia.
    ///  Reference: https://www.safework.sa.gov.au/resources/public-holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsSaHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetSaHolidays(date.Year);
        name = holidays
            .Where(_ => _.date == date)
            .Select(_ => _.name)
            .SingleOrDefault();
        return name != null;
    }

    /// <summary>
    ///  Gets all public holidays for South Australia.
    ///  Reference: https://www.safework.sa.gov.au/resources/public-holidays
    /// </summary>
    public static IEnumerable<(Date date, string name)> GetSaHolidays(int year)
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

        yield return (Extensions.GetSecondMonday(Month.March, year), "Adelaide Cup Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (new(year, (int) Month.April, 25), "Anzac Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(Month.October, year), "Labour Day");

        yield return (ChristmasCalculator.ChristmasEve(year), "Christmas Eve (partial day)");
        yield return (ChristmasCalculator.ChristmasDay(year), "Christmas Day");

        var proclamationDay = new Date(year, (int) Month.December,26);
        if (proclamationDay.IsWeekday())
        {
            if (proclamationDay.IsWeekday())
            {
                yield return (proclamationDay, "Proclamation Day and Boxing Day");
            }
            else
            {
                yield return (proclamationDay, "Proclamation Day");
            }
        }
        else if(proclamationDay.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (proclamationDay, "Proclamation Day");
            yield return (proclamationDay.AddDays(2), "Proclamation Day (additional)");
        }
        else if(proclamationDay.DayOfWeek == DayOfWeek.Sunday)
        {
            yield return (proclamationDay, "Proclamation Day");
            yield return (proclamationDay.AddDays(1), "Proclamation Day (additional)");
        }
    }
}