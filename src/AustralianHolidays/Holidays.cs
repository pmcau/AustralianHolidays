namespace AustralianHolidays;

public static partial class Holidays
{
    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;

    public static bool IsPublicHoliday(this Date date, State state) => IsPublicHoliday(date, state, out _);

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

    static bool IsFirstMonday(Date date, Month month)
    {
        var dateYear = date.Year;
        var firstDay = new Date(dateYear, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        var firstMonday = firstDay.AddDays(daysUntilMonday);
        return date == firstMonday;
    }

    static bool IsSecondMonday(Date date, Month month)
    {
        var firstDay = new Date(date.Year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        var secondMonday = firstDay.AddDays(daysUntilMonday + 7);
        return date == secondMonday;
    }
}