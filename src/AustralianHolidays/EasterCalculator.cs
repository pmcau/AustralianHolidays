namespace AustralianHolidays;

public static class EasterCalculator
{
    public static bool TryGetPublicHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        var (easterFriday, easterSunday, easterMonday) = ForYear(date.Year);
        if (date == easterFriday)
        {
            name = "Easter Friday";
            return true;
        }

        if (date == easterMonday)
        {
            name = "Easter Monday";
            return true;
        }

        if (date == easterSunday)
        {
            name = "Easter Sunday";
            return true;
        }

        name = null;
        return false;
    }

    public static (Date friday, Date sunday, Date monday) ForYear(int year)
    {
        var sunday = GetEasterSunday(year);
        var friday = sunday.AddDays(-2);
        var monday = sunday.AddDays(1);
        return (friday, sunday, monday);
    }

    public static Date GetEasterMonday(int year) =>
        GetEasterSunday(year).AddDays(1);

    public static bool IsEasterMonday(this Date date) =>
        GetEasterMonday(date.Year) == date;

    public static Date GetEasterFriday(int year) =>
        GetEasterSunday(year).AddDays(-2);

    public static bool IsEasterFriday(this Date date) =>
        GetEasterFriday(date.Year) == date;

    public static bool IsEasterSunday(this Date date) =>
        GetEasterSunday(date.Year) == date;

    // computus algorithm
    public static Date GetEasterSunday(int year)
    {
        var month = 3;
        //19 year metonic cycle
        var goldenNumber = year % 19;
        var century = year / 100;
        // Easter Sunday is never celebrated on 21 March since it is the vernal equinox
        // https://earthsky.org/astronomy-essentials/easter-full-moon-vernal-equinox/
        // ReSharper disable once ArrangeRedundantParentheses
        var daysFromMarch21ToFullMoon = (century - (century / 4) - (8 * century + 13) / 25 + 19 * goldenNumber + 15) % 30;
        //handle edge cases
        var adjustedDaysFromMatch21ToFullMoon = daysFromMarch21ToFullMoon - daysFromMarch21ToFullMoon / 28 * (1 - daysFromMarch21ToFullMoon / 28 * (29 / (daysFromMarch21ToFullMoon + 1)) * ((21 - goldenNumber) / 11));
        var day = adjustedDaysFromMatch21ToFullMoon - (year + year / 4 + adjustedDaysFromMatch21ToFullMoon + 2 - century + century / 4) % 7 + 28;
        if (day > 31)
        {
            month = 4;
            day -= 31;
        }

        return new(year, month, day);
    }
}