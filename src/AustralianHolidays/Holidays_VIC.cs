namespace AustralianHolidays;

public static partial class Holidays
{
    //https://www.wa.gov.au/service/employment/workplace-arrangements/public-holidays-western-australia
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
                name = "Australia Day Holiday";
                return true;
            }
        }

        if (date.Month == 3)
        {
            var firstDayOfMarch = new Date(date.Year, 3, 1);
            var dayOfWeek = (int)firstDayOfMarch.DayOfWeek;
            var daysUntilMonday = (8 - dayOfWeek) % 7;
            var secondMonday = firstDayOfMarch.AddDays(daysUntilMonday + 7);
            if (date == secondMonday)
            {
                name = "Labour Day";
                return true;
            }
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

        if (ChristmasCalculator.TryGet(date, out name))
        {
            return true;
        }

        name = null;
        return false;
    }
}