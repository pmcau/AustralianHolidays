namespace AustralianHolidays;

public static partial class Holidays
{
    //https://www.cmtedd.act.gov.au/communication/holidays
    public static bool IsNtHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsNewYearsDay())
        {
            name = "New Year's Day";
            return true;
        }

        if (ChristmasCalculator.TryGet(date, out name))
        {
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

        if (date.Month == 5)
        {
            var firstDayOfMay = new Date(date.Year, 5, 1);
            var dayOfWeek = (int)firstDayOfMay.DayOfWeek;
            var daysUntilMonday = (8 - dayOfWeek) % 7;
            var firstMonday = firstDayOfMay.AddDays(daysUntilMonday);
            if (date == firstMonday)
            {
                name = "May Day";
                return true;
            }
        }
        if (date.Month == 8)
        {
            var firstDayOfAugust = new Date(date.Year, 8, 1);
            var dayOfWeek = (int)firstDayOfAugust.DayOfWeek;
            var daysUntilMonday = (8 - dayOfWeek) % 7;
            var firstMonday = firstDayOfAugust.AddDays(daysUntilMonday);
            if (date == firstMonday)
            {
                name = "Picnic Day";
                return true;
            }
        }

        if (date.IsAnzacDay())
        {
            name = "Anzac Day";
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

        name = null;
        return false;
    }
}