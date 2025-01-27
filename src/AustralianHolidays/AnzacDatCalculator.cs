namespace AustralianHolidays;

public static class AnzacDatCalculator
{
    public static bool IsAnzacDay(this Date date) =>
        date is { Month: 4, Day: 25 } &&
        date.IsWeekday();

    public static bool IsAnzacDay2(this Date date)
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