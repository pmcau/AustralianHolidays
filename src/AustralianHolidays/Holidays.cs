namespace AustralianHolidays;

public static partial class Holidays
{
    static readonly State[] states;

    static Holidays()
    {
        states = Enum.GetValues<State>();
        PopulateCache(DateTime.Now.Year - 1, 12);
    }

    [MemberNotNull(nameof(cache))]
    [MemberNotNull(nameof(actCache))]
    [MemberNotNull(nameof(waCache))]
    [MemberNotNull(nameof(nswCache))]
    [MemberNotNull(nameof(saCache))]
    [MemberNotNull(nameof(ntCache))]
    [MemberNotNull(nameof(tasCache))]
    [MemberNotNull(nameof(vicCache))]
    [MemberNotNull(nameof(qldCache))]
    public static void PopulateCache(int startYear, int count)
    {
        cache = new(BuildForMultipleYears(BuildHolidays, startYear, count));
        actCache = new(BuildForMultipleYears(BuildActHolidays, startYear, count));
        waCache = new(BuildForMultipleYears(BuildWaHolidays, startYear, count));
        nswCache = new(BuildForMultipleYears(BuildNswHolidays, startYear, count));
        saCache = new(BuildForMultipleYears(BuildSaHolidays, startYear, count));
        ntCache = new(BuildForMultipleYears(BuildNtHolidays, startYear, count));
        tasCache = new(BuildForMultipleYears(BuildTasHolidays, startYear, count));
        vicCache = new(BuildForMultipleYears(BuildVicHolidays, startYear, count));
        qldCache = new(BuildForMultipleYears(BuildQldHolidays, startYear, count));
    }

    static IEnumerable<KeyValuePair<int, Dictionary<Date, string>>> BuildForMultipleYears(Func<int, IEnumerable<(Date date, string name)>> action, int startYear, int count)
    {
        var lastYear = startYear + count - 1;
        for (var year = startYear; year <= lastYear; year++)
        {
            yield return new(year, action(year).ToDictionary(_ => _.date, _ => _.name));
        }
    }

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
        var action = DeriveGetHolidaysAction(state);
        List<(Date date, string name)> list = [];
        for (var year = startYear; year <= startYear + yearCount -1; year++)
        {
            foreach (var (key, value) in action(year))
            {
                list.Add((key, value));
            }
        }

        return list.OrderBy(_ => _.date);
    }

    public static IOrderedEnumerable<(Date date, string name)> ForYearsFederal(int startYear, int yearCount = 1)
    {
        List<(Date date, string name)> list = [];
        for (var year = startYear; year <= startYear + yearCount -1; year++)
        {
            foreach (var (key, value) in GetHolidays(year))
            {
                list.Add((key, value));
            }
        }

        return list.OrderBy(_ => _.date);
    }

    static Func<int, IReadOnlyDictionary<Date, string>> DeriveGetHolidaysAction(State state) =>
        state switch
        {
            State.ACT => GetActHolidays,
            State.NSW => GetNswHolidays,
            State.NT => GetNtHolidays,
            State.QLD => GetQldHolidays,
            State.SA => GetSaHolidays,
            State.TAS => GetTasHolidays,
            State.VIC => GetVicHolidays,
            State.WA => GetWaHolidays,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;

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

    static Date GetAustraliaDay(int year) => new(year, January, 26);
}