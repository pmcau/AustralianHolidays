using AustralianHolidays;

public class HolidayService
{
    public virtual IOrderedEnumerable<(Date date, State state, string name)> ForYears(int? startYear = null, int yearCount = 1) =>
        Holidays.ForYears(startYear, yearCount);

    public virtual IOrderedEnumerable<(Date date, string name)> ForYears(State state, int? startYear = null, int yearCount = 1) =>
        Holidays.ForYears(state, startYear, yearCount);

    public virtual IOrderedEnumerable<(Date date, string name)> ForYearsFederal(int? startYear = null, int yearCount = 1) =>
        Holidays.ForYearsFederal(startYear, yearCount);

    public virtual bool IsHoliday(Date date, State state) =>
        date.IsHoliday(state);

    public virtual bool IsHoliday(Date date, State state, [NotNullWhen(true)] out string? name) =>
        date.IsHoliday(state, out name);

    public virtual bool IsActHoliday(Date date) =>
        date.IsActHoliday();

    public virtual bool IsActHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsActHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetActHolidays(int year) =>
        Holidays.GetActHolidays(year);

    public virtual bool IsNationalHoliday(Date date) =>
        date.IsNationalHoliday();

    public virtual bool IsNationalHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsNationalHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> NationalForYears(int year) =>
        Holidays.NationalForYears(year);

    public virtual bool IsFederalGovernmentShutdown(Date date) =>
        date.IsFederalGovernmentShutdown();

    public virtual (Date start, Date end) GetFederalGovernmentShutdown(int startYear) =>
        Holidays.GetFederalGovernmentShutdown(startYear);

    public virtual string ExportToMarkdown(int? startYear = null, int yearCount = 5) =>
        Holidays.ExportToMarkdown(startYear, yearCount);

    public virtual string ExportToMarkdown(State state, int? startYear = null, int yearCount = 5) =>
        Holidays.ExportToMarkdown(state, startYear, yearCount);

    public virtual bool IsNswHoliday(Date date) =>
        date.IsNswHoliday();

    public virtual bool IsNswHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsNswHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetNswHolidays(int year) =>
        Holidays.GetNswHolidays(year);

    public virtual bool IsNtHoliday(Date date) =>
        date.IsNtHoliday();

    public virtual bool IsNtHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsNtHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetNtHolidays(int year) =>
        Holidays.GetNtHolidays(year);

    public virtual bool IsQldHoliday(Date date) =>
        date.IsQldHoliday();

    public virtual bool IsQldHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsQldHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetQldHolidays(int year) =>
        Holidays.GetQldHolidays(year);

    public virtual bool IsSaHoliday(Date date) =>
        date.IsSaHoliday();

    public virtual bool IsSaHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsSaHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetSaHolidays(int year) =>
        Holidays.GetSaHolidays(year);

    public virtual bool IsTasHoliday(Date date) =>
        date.IsTasHoliday();

    public virtual bool IsTasHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsTasHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetTasHolidays(int year) =>
        Holidays.GetTasHolidays(year);

    public virtual bool IsVicHoliday(Date date) =>
        date.IsVicHoliday();

    public virtual bool IsVicHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsVicHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetVicHolidays(int year) =>
        Holidays.GetVicHolidays(year);

    public virtual bool IsWaHoliday(Date date) =>
        date.IsWaHoliday();

    public virtual bool IsWaHoliday(Date date, [NotNullWhen(true)] out string? name) =>
        date.IsWaHoliday(out name);

    public virtual IReadOnlyDictionary<Date, string> GetWaHolidays(int year) =>
        Holidays.GetWaHolidays(year);
}