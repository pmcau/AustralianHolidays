namespace AustralianHolidays;

public static partial class Holidays
{
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
        if (date.IsNewYearsDay())
        {
            name = "New Year's Day";
            return true;
        }

        if (date.Month == 1)
        {
            if (date.Day == 26 && date.IsWeekday())
            {
                name = "Australia Day";
                return true;
            }

            if (date is { DayOfWeek: DayOfWeek.Monday, Day: 27 or 28 })
            {
                name = "Australia Day (additional)";
                return true;
            }
        }

        if (date.IsSecondMonday(Month.March))
        {
            name = "Labour Day";
            return true;
        }

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(date.Year);
        if (date == easterFriday)
        {
            name = "Good Friday";
            return true;
        }

        if (date == easterSaturday)
        {
            name = "Easter Saturday";
            return true;
        }

        if (date == easterSunday)
        {
            name = "Easter Sunday";
            return true;
        }

        if (date == easterMonday)
        {
            name = "Easter Monday";
            return true;
        }

        if (date.IsAnzacDay())
        {
            name = "Anzac Day";
            return true;
        }

        if (date.IsMonarchBirthday(out name))
        {
            return true;
        }

        if (date.IsLastFridayInMonth(Month.September))
        {
            name = "Friday before AFL Grand Final (Subject to AFL schedule)";
            return true;
        }

        if (date.IsFirstTuesday(Month.November))
        {
            name = "Melbourne Cup Day";
            return true;
        }

        if (ChristmasCalculator.TryGet(date, out name))
        {
            return true;
        }

        name = null;
        return false;
    }
}