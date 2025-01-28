namespace AustralianHolidays;

public static class Holidays
{
    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;

    internal static bool IsPublicHoliday(this Date date) => IsPublicHoliday(date, out _);

    internal static bool IsPublicHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date is { Month: 1, Day: 1 })
        {
            name = "New Year's Day";
            return true;
        }

        var christmas = new Date(date.Year, 12, 25);
        if (date == christmas)
        {
            name = "Christmas Day";
            return true;
        }

        var nextNewYears = GetNewYears(date.Year + 1);

        if (date > christmas && date < nextNewYears)
        {
            name = "Government shutdown";
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
                name = "Australia Day";
                return true;
            }
        }

        if (date.Month == 4)
        {
            if (date.Day == 25 && date.IsWeekday())
            {
                name = "Anzac Day";
                return true;
            }

            if (date is { DayOfWeek: DayOfWeek.Monday, Day: 26 or 27 })
            {
                name = "Anzac Day";
                return true;
            }
        }

        if (EasterCalculator.TryGetPublicHoliday(date, out name))
        {
            return true;
        }

        name = null;
        return false;
    }


    static Date GetNewYears(int year)
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