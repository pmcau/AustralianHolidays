namespace AustralianHolidays;

public static class EasterCalculator
{
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

    // Computus algorithm - calculates Easter Sunday using astronomical/calendar calculations
    static Date GetEasterSunday(int year)
    {
        // Step 1: Calculate the golden number (position in the 19-year Metonic cycle)
        var goldenNumber = year % 19;

        // Step 2: Century-based calculations
        var century = year / 100;
        var centuryLeapYearCorrection = century / 4;
        var lunarOrbitCorrection = (8 * century + 13) / 25;

        // Step 3: Calculate the epact (age of the moon on January 1)
        // This determines how many days after March 21 the full moon occurs
        // ReSharper disable once ArrangeRedundantParentheses
        var epact = (century - centuryLeapYearCorrection - lunarOrbitCorrection + 19 * goldenNumber + 15) % 30;

        // Step 4: Adjust the epact for special cases (Clavian and Julian corrections)
        // These corrections ensure Easter doesn't fall on specific problematic dates
        var epactAdjustmentFactor = epact / 28;
        var additionalCorrection = 1 - epactAdjustmentFactor * (29 / (epact + 1)) * ((21 - goldenNumber) / 11);
        var adjustedEpact = epact - epactAdjustmentFactor * additionalCorrection;

        // Step 5: Calculate day of week offset to find the following Sunday
        var yearDayOffset = year + year / 4;
        var weekdayCorrection = (yearDayOffset + adjustedEpact + 2 - century + centuryLeapYearCorrection) % 7;

        // Step 6: Calculate the final day (starting from March 28 as a reference point)
        // Easter Sunday is never celebrated on March 21 since it is the vernal equinox
        // https://earthsky.org/astronomy-essentials/easter-full-moon-vernal-equinox/
        var day = adjustedEpact - weekdayCorrection + 28;

        // Step 7: Determine if Easter falls in March or April
        var month = 3;
        if (day > 31)
        {
            month = 4;
            day -= 31;
        }

        return new(year, month, day);
    }
}
