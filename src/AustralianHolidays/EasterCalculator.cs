namespace AustralianHolidays;

public static class EasterCalculator
{
    //https://www.clockon.com.au/blog/easter-public-holidays-across-australian-states-differences
    public static bool TryGetPublicHoliday(Date date, State state, [NotNullWhen(true)] out string? name)
    {
        var (easterFriday, easterSaturday, easterSunday, easterMonday) = ForYear(date.Year);
        if (date == easterFriday)
        {
            name = "Good Friday";
            return true;
        }

        if (date == easterSaturday)
        {
            if (state != State.TAS &&
                state != State.WA)
            {
                name = "Easter Saturday";
                return true;
            }

            name = null;
            return false;
        }

        if (date == easterSunday)
        {
            if (state == State.TAS)
            {
                name = null;
                return false;
            }

            if (state == State.WA && date.Year >= 2022)
            {
                name = null;
                return false;
            }

            name = "Easter Sunday";
            return true;
        }

        if (date == easterMonday)
        {
            name = "Easter Monday";
            return true;
        }

        if (date == easterMonday.AddDays(1))
        {
            if (state == State.TAS)
            {
                name = "Easter Tuesday (Government employees only)";
                return true;
            }

            name = null;
            return false;
        }

        name = null;
        return false;
    }

    public static (Date friday, Date saturday, Date sunday, Date monday) ForYear(int year)
    {
        var sunday = GetEasterSunday(year);
        var friday = sunday.AddDays(-2);
        var saturday = sunday.AddDays(-1);
        var monday = sunday.AddDays(1);
        return (friday, saturday, sunday, monday);
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