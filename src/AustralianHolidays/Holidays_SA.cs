namespace AustralianHolidays;

public static partial class Holidays
{
    //https://www.safework.sa.gov.au/resources/public-holidays
    public static bool IsSaHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsNewYearsDay())
        {
            name = "New Year's Day";
            return true;
        }

        if (date.Month == 1 &&
            date is { DayOfWeek: DayOfWeek.Monday, Day: 27 or 28 })
        {
            name = "Australia Day Holiday";
            return true;
        }

        if (IsSecondMonday(date, Month.March))
        {
            name = "Adelaide Cup Day";
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

        if (IsFirstMonday(date, Month.October))
        {
            name = "Labour Day";
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