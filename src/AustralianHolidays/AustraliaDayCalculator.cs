namespace AustralianHolidays;

public static class AustraliaDayCalculator
{
    public  static bool TryGetPublicHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.Month == 1)
        {
            if (date.Day == 26 && date.IsWeekday())
            {
                //not NSW
                //TODO: not really a public holiday??
                name = "Australia Day";
                return true;
            }

            if (date is { DayOfWeek: DayOfWeek.Monday, Day: 27 or 28 })
            {
                name = "Australia Day Holiday";
                return true;
            }
        }

        name = null;
        return false;
    }
}