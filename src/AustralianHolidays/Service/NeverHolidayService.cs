namespace AustralianHolidays;

public class NeverHolidayService() :
    HolidayService(TimeProvider.System)
{
    public override IOrderedEnumerable<(Date date, State state, string name)> ForYears(int? startYear = null, int yearCount = 1) =>
        Enumerable.Empty<(Date date, State state, string name)>().OrderBy(_ => _.date);

    public override IOrderedEnumerable<(Date date, string name)> ForYears(State state, int? startYear = null, int yearCount = 1) =>
        Enumerable.Empty<(Date date, string name)>().OrderBy(_ => _.date);

    public override IOrderedEnumerable<(Date date, string name)> NationalForYears(int? startYear = null, int yearCount = 1) =>
        Enumerable.Empty<(Date date, string name)>().OrderBy(_ => _.date);

    static bool IsHoliday(out string? name)
    {
        name = null;
        return false;
    }

    public override bool IsHoliday(Date date, State state) => false;

    public override bool IsHoliday(Date date, State state, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override bool IsActHoliday(Date date) => false;

    public override bool IsActHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForAct(int year) => new Dictionary<Date, string>();

    public override bool IsNationalHoliday(Date date) => false;

    public override bool IsNationalHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForNational(int year) => new Dictionary<Date, string>();

    public override bool IsFederalGovernmentShutdown(Date date) => false;

    public override (Date start, Date end) GetFederalGovernmentShutdown(int startYear) => (Date.MinValue, Date.MinValue);

    public override string ExportToMarkdown(int? startYear = null, int yearCount = 5) => string.Empty;

    public override string ExportToMarkdown(State state, int? startYear = null, int yearCount = 5) => string.Empty;

    public override bool IsNswHoliday(Date date) => false;

    public override bool IsNswHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForNsw(int year) => new Dictionary<Date, string>();

    public override bool IsNtHoliday(Date date) => false;

    public override bool IsNtHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForNt(int year) => new Dictionary<Date, string>();

    public override bool IsQldHoliday(Date date) => false;

    public override bool IsQldHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForQld(int year) => new Dictionary<Date, string>();

    public override bool IsSaHoliday(Date date) => false;

    public override bool IsSaHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForSa(int year) => new Dictionary<Date, string>();

    public override bool IsTasHoliday(Date date) => false;

    public override bool IsTasHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForTas(int year) => new Dictionary<Date, string>();

    public override bool IsVicHoliday(Date date) => false;

    public override bool IsVicHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForVic(int year) => new Dictionary<Date, string>();

    public override bool IsWaHoliday(Date date) => false;

    public override bool IsWaHoliday(Date date, [NotNullWhen(true)] out string? name) => IsHoliday(out name);

    public override IReadOnlyDictionary<Date, string> ForWa(int year) => new Dictionary<Date, string>();
}