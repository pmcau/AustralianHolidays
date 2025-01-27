namespace AustralianHolidays;

public static class AnzacDatCalculator
{
    public static bool IsAnzacDay(this Date date)
    {
        if (date.Month == 4)
        {
            if (date.Day == 25 && date.IsWeekday())
            {
                return true;
            }

            if (date is { DayOfWeek: DayOfWeek.Monday, Day: 26 or 27 })
            {
                return true;
            }
        }

        return false;
    }
}