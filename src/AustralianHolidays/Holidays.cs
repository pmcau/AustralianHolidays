namespace AustralianHolidays;

public static partial class Holidays
{
    static readonly State[] states;

    static Holidays() =>
        states = Enum.GetValues<State>();

    public static IOrderedEnumerable<(Date date, State state, string name)> ForYears(int startYear, int yearCount = 1)
    {
        List<(Date date, State state, string name)> list = [];
        for (var year = startYear; year <= startYear + yearCount; year++)
        {
            foreach (var date in GetAllDatesForYear(year))
            {
                foreach (var state in states)
                {
                    if (date.IsHoliday(state, out var name))
                    {
                        list.Add((date, state, name));
                    }
                }
            }
        }

        return list.OrderBy(_ => _.date);
    }

    public static IOrderedEnumerable<(Date date, string name)> ForYears(State state, int startYear, int yearCount = 1)
    {
        List<(Date date, string name)> list = [];
        for (var year = startYear; year <= startYear + yearCount; year++)
        {
            foreach (var date in GetAllDatesForYear(year))
            {
                if (date.IsHoliday(state, out var name))
                {
                    list.Add((date, name));
                }
            }
        }

        return list.OrderBy(_ => _.date);
    }

    static IEnumerable<Date> GetAllDatesForYear(int year)
    {
        var startDate = new Date(year, 1, 1);
        var endDate = new Date(year, 12, 31);

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;

    internal static bool IsWeekEnd(this Date date) =>
        date.DayOfWeek is
            DayOfWeek.Saturday or
            DayOfWeek.Sunday;

    public static bool IsHoliday(this Date date, State state) => IsHoliday(date, state, out _);

    public static bool IsHoliday(this Date date, State state, [NotNullWhen(true)] out string? name)
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
        date is {Month: 1, Day: 1};

    static bool IsNewYearsEve(this Date date) =>
        date is {Month: 12, Day: 31};

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