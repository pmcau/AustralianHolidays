namespace AustralianHolidays;

public static partial class Holidays
{
    //https://www.cmtedd.act.gov.au/communication/holidays
    public static bool IsTasHoliday(this Date date, [NotNullWhen(true)] out string? name)
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

        if (date.Month == 3)
        {
            var firstDayOfMarch = new Date(date.Year, 3, 1);
            var dayOfWeek = (int)firstDayOfMarch.DayOfWeek;
            var daysUntilMonday = (8 - dayOfWeek) % 7;
            var secondMonday = firstDayOfMarch.AddDays(daysUntilMonday + 7);
            if (date == secondMonday)
            {
                name = "Eight Hours Day";
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

        if (date == easterMonday.AddDays(1))
        {
            name = "Easter Tuesday (Government employees only)";
            return true;
        }

        name = null;
        return false;
    }

    static bool IsSecondMondayInMarch(Date date)
    {
        if (date.Month == 3)
        {
            var firstDayOfMonth = new Date(date.Year, 3, 1);
            if (firstDayOfMonth.DayOfWeek == DayOfWeek.Monday)
            {
                return date.Day is >= 8 and <= 14;
            }

            return date.Day is >= 9 and <= 15;
        }

        return false;
    }
}