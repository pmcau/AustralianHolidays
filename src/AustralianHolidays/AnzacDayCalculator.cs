namespace AustralianHolidays;

public static class AnzacDayCalculator
{
    public static bool TryGetPublicHoliday(Date date, State state, [NotNullWhen(true)] out string? name)
    {
        if (date.Month == 4)
        {
            if (date.Day == 25)
            {
                name = "Anzac Day";
                return true;
            }

            // Anzac Day falls on a Saturday
            if (date is { DayOfWeek: DayOfWeek.Monday, Day: 27 })
            {
                if (state == State.WA)
                {
                    name = "Anzac Day Holiday";
                    return true;
                }
            }

            // Anzac Day falls on a Sunday
            if (date is { DayOfWeek: DayOfWeek.Monday, Day: 26 })
            {
                if (state is State.NSW or State.TAS or State.VIC)
                {
                    name = null;
                    return false;
                }

                name = "Anzac Day Holiday";
                return true;
            }
        }

        name = null;
        return false;
    }

    public static bool IsAnzacDay(this Date date) =>
        date is { Month: 4, Day: 25 };
}