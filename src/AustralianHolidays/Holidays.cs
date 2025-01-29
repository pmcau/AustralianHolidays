namespace AustralianHolidays;

public static partial class Holidays
{
    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;
    internal static bool IsWeekEnd(this Date date) =>
        date.DayOfWeek is
            DayOfWeek.Saturday or
            DayOfWeek.Sunday;

    public static bool IsPublicHoliday(this Date date, State state) => IsPublicHoliday(date, state, out _);

    public static bool IsPublicHoliday(this Date date, State state, [NotNullWhen(true)] out string? name)
    {
        switch (state)
        {
            case State.NSW:
                return IsNswHoliday(date, out name);
            case State.VIC:
                return IsVicHoliday(date, out name);
            case State.QLD:
                return IsQldHoliday(date, out name);
            case State.ACT:
                return IsActHoliday(date, out name);
            case State.NT:
                return IsNtHoliday(date, out name);
            case State.SA:
                return IsSaHoliday(date, out name);
            case State.TAS:
                return IsTasHoliday(date, out name);
            case State.WA:
                return IsWaHoliday(date, out name);
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    static bool IsNewYearsDay(this Date date) =>
        date is { Month: 1, Day: 1 };

    static bool IsNewYearsEve(this Date date) =>
        date is { Month: 12, Day: 31 };

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