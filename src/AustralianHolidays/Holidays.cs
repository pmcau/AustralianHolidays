namespace AustralianHolidays;

public static partial class Holidays
{
    static readonly State[] states;

    static Holidays() =>
        states = Enum.GetValues<State>();

    public static IOrderedEnumerable<(Date date, State state, string name)> ForYears(int startYear, int yearCount = 1)
    {
        List<(Date date, State state, string name)> list = [];
        foreach (var state in states)
        {
            foreach (var (date, name) in ForYears(state, startYear, yearCount))
            {
                list.Add((date, state, name));
            }
        }

        return list.OrderBy(_ => _.date);
    }

    public static IOrderedEnumerable<(Date date, string name)> ForYears(State state, int startYear, int yearCount = 1)
    {
        var action = GetStateAction(state);
        List<(Date date, string name)> list = [];
        for (var year = startYear; year <= startYear + yearCount -1; year++)
        {
            foreach (var (key, value) in action(year))
            {
#if DEBUG
                if (list.Any(_ => _.date == key && _.name == value))
                {
                    throw new InvalidOperationException($"Duplicate: {key} {value}");
                }
#endif
                list.Add((key, value));
            }
        }

        return list.OrderBy(_ => _.date);
    }

    static Func<int, IReadOnlyDictionary<Date, string>> GetStateAction(State state)
    {
        Func<int, IReadOnlyDictionary<Date,string>> action;
        switch (state)
        {
            case State.ACT:
                action = GetActHolidays;
                break;
            case State.NSW:
                action = GetNswHolidays;
                break;
            case State.NT:
                action = GetNtHolidays;
                break;
            case State.QLD:
                action = GetQldHolidays;
                break;
            case State.SA:
                action = GetSaHolidays;
                break;
            case State.TAS:
                action = GetTasHolidays;
                break;
            case State.VIC:
                action = GetVicHolidays;
                break;
            case State.WA:
                action = GetWaHolidays;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        return action;
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

    public static bool IsHoliday(this Date date, State state, [NotNullWhen(true)] out string? name) =>
        state switch
        {
            State.NSW => IsNswHoliday(date, out name),
            State.VIC => IsVicHoliday(date, out name),
            State.QLD => IsQldHoliday(date, out name),
            State.ACT => IsActHoliday(date, out name),
            State.NT => IsNtHoliday(date, out name),
            State.SA => IsSaHoliday(date, out name),
            State.TAS => IsTasHoliday(date, out name),
            State.WA => IsWaHoliday(date, out name),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

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