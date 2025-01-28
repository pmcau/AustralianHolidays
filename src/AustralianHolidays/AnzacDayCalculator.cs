namespace AustralianHolidays;

public static class AnzacDayCalculator
{
    public static bool IsAnzacDay(this Date date) =>
        date is { Month: 4, Day: 25 };

    public static bool IsAnzacDayHoliday(this Date date, Area area)
    {
        if (date.Month != 4)
        {
            return false;
        }

        if (date.Day == 25)
        {
            return true;
        }

        // Anzac Day falls on a Saturday
        if (date is { DayOfWeek: DayOfWeek.Monday, Day: 27 })
        {
            return area == Area.WA;
        }

        // Anzac Day falls on a Sunday
        if (date is { DayOfWeek: DayOfWeek.Monday, Day: 26 })
        {
            return area != Area.NSW &&
                   area != Area.ACT &&
                   area != Area.VIC;
        }

        return false;
    }
}