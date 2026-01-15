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

    static IEnumerable<KeyValuePair<int, FrozenDictionary<Date, string>>> BuildForMultipleYears(Func<int, IEnumerable<(Date date, string name)>> action, int startYear, int count)
    {
        var lastYear = startYear + count - 1;
        for (var year = startYear; year <= lastYear; year++)
        {
            yield return new(
                year,
                action(year)
                    .ToFrozenDictionary(_ => _.date, _ => _.name));
        }
    }

    /// <summary>
    /// Retrieves public holidays for all states over a specified range of years.
    /// </summary>
    /// <param name="startYear">The starting year for the range. If not provided, the current year is used.</param>
    /// <param name="yearCount">The number of years to include in the range. Defaults to 1 year.</param>
    /// <returns>An ordered enumerable of tuples containing the date, state, and name of each public holiday.</returns>
    public static IOrderedEnumerable<(Date date, State state, string name)> ForYears(int? startYear = null, int yearCount = 1)
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

    /// <summary>
    /// Retrieves public holidays for a specified state over a given range of years.
    /// </summary>
    /// <param name="state">The state for which to retrieve public holidays.</param>
    /// <param name="startYear">The starting year for the range. If not provided, the current year is used.</param>
    /// <param name="yearCount">The number of years to include in the range. Defaults to 1 year.</param>
    /// <returns>An ordered enumerable of tuples containing the date and name of each public holiday in the specified state.</returns>
    public static IOrderedEnumerable<(Date date, string name)> ForYears(State state, int? startYear = null, int yearCount = 1)
    {
        var start = OrCurrentYear(startYear);
        var action = DeriveGetHolidaysAction(state);
        List<(Date date, string name)> list = [];
        for (var year = start; year <= start + yearCount - 1; year++)
        {
            foreach (var (key, value) in action(year))
            {
                list.Add((key, value));
            }
        }

        return list.OrderBy(_ => _.date);
    }

    static int OrCurrentYear(int? year) =>
        year ?? DateTime.Now.Year;

    public static IOrderedEnumerable<(Date date, string name)> NationalForYears(int? startYear = null, int yearCount = 1)
    {
        var start = OrCurrentYear(startYear);
        List<(Date date, string name)> list = [];
        for (var year = start; year <= start + yearCount - 1; year++)
        {
            foreach (var (key, value) in ForNational(year))
            {
                list.Add((key, value));
            }
        }

        return list.OrderBy(_ => _.date);
    }

    static Func<int, IReadOnlyDictionary<Date, string>> DeriveGetHolidaysAction(State state) =>
        state switch
        {
            ACT => ForAct,
            NSW => ForNsw,
            NT => ForNt,
            QLD => ForQld,
            SA => ForSa,
            TAS => ForTas,
            VIC => ForVic,
            WA => ForWa,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

    internal static bool IsWeekday(this Date date) =>
        date.DayOfWeek is
            not DayOfWeek.Saturday and
            not DayOfWeek.Sunday;

    /// <summary>
    /// Determines if a given date is a public holiday in a specified state.
    /// </summary>
    public static bool IsHoliday(this Date date, State state) =>
        date.IsHoliday(state, out _);

    /// <summary>
    /// Determines if a specific date is a recognized public holiday in a particular state and retrieving the name of the holiday if it is.
    /// </summary>
    public static bool IsHoliday(this Date date, State state, [NotNullWhen(true)] out string? name) =>
        state switch
        {
            NSW => date.IsNswHoliday(out name),
            VIC => date.IsVicHoliday(out name),
            QLD => date.IsQldHoliday(out name),
            ACT => date.IsActHoliday(out name),
            NT => date.IsNtHoliday(out name),
            SA => date.IsSaHoliday(out name),
            TAS => date.IsTasHoliday(out name),
            WA => date.IsWaHoliday(out name),
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

    internal static List<int> BuildYears(int? startYear)
    {
        var start = OrCurrentYear(startYear);
        List<int> years = [];
        for (var year = start; year <= start + 4; year++)
        {
            years.Add(year);
        }

        return years;
    }
}
