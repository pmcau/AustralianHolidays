namespace AustralianHolidays;

public static class Holidays
{
    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;

    public static bool IsPublicHoliday(this Date date, State state) => IsPublicHoliday(date,state, out _);

    public static bool IsPublicHoliday(this Date date, State state, [NotNullWhen(true)] out string? name)
    {
        if (date.IsNewYearsDay())
        {
            name = "New Year's Day";
            return true;
        }

        if (date.IsChristmasDay())
        {
            name = "Christmas Day";
            return true;
        }

        if (AustraliaDayCalculator.TryGetPublicHoliday(date, out name))
        {
            return true;
        }

        if (AnzacDayCalculator.TryGetPublicHoliday(date, state, out name))
        {
            return true;
        }

        if (EasterCalculator.TryGetPublicHoliday(date, state, out name))
        {
            return true;
        }

        name = null;
        return false;
    }

    public static bool IsNswHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsNewYearsDay())
        {
            name = "New Year's Day";
            return true;
        }

        var christmasDay = ChristmasDay(date.Year);
        if (date == christmasDay)
        {
            name = "Christmas Day";
            return true;
        }

        var boxingDay = BoxingDay(date.Year);
        if (date == boxingDay)
        {
            name = "Boxing Day";
            return true;
        }
        var boxingDayPlus1 = boxingDay.AddDays(1);
        if (date == boxingDayPlus1 && boxingDayPlus1.IsWeekday())
        {
            name = "Christmas Holiday";
            return true;
        }

        var boxingDayPlus2 = boxingDay.AddDays(1);
        if (date == boxingDayPlus2 && boxingDayPlus2.IsWeekday())
        {
            name = "Christmas Holiday";
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

    public  static bool IsChristmasDay(this Date date) =>
        date == ChristmasDay(date.Year);

    public static Date ChristmasDay(int year) =>
        new(year, 12, 25);

    public static Date BoxingDay(int year) =>
        new(year, 12, 26);

    public static bool IsBoxingDay(this Date date) =>
        date == BoxingDay(date.Year);

    static bool IsNewYearsDay(this Date date) =>
        date is { Month: 1, Day: 1 };

    public static bool IsFederalGovernmentShutdown(this Date date)
    {
        var christmas = new Date(date.Year, 12, 25);

        var newYearsHoliday = GetNewYearsHoliday(date.Year + 1);

        return date >= christmas &&
               date <= newYearsHoliday;
    }

    static Date GetNewYearsHoliday(int year)
    {
        var oneJanuary = new Date(year, 1, 1);

        if (oneJanuary.DayOfWeek is
            DayOfWeek.Saturday)
        {
            return new(year, 1, 3);
        }

        if (oneJanuary.DayOfWeek is
            DayOfWeek.Sunday)
        {
            return new(year, 1, 2);
        }

        return oneJanuary;
    }
}