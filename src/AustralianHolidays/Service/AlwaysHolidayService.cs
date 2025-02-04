namespace AustralianHolidays;

public class AlwaysHolidayService(TimeProvider? timeProvider = null) :
    HolidayService(timeProvider?? TimeProvider.System)
{
    public override IOrderedEnumerable<(Date date, State state, string name)> ForYears(int? startYear = null, int yearCount = 1) =>
        GetDates(startYear, yearCount)
            .SelectMany(date => Enum.GetValues<State>()
                .Select(state => (date, state, "Holiday")))
            .OrderBy(_ => _.date);

    IEnumerable<Date> GetDates(int? startYear, int yearCount)
    {
        var start = startYear ?? TimeProvider.GetLocalNow().Year;
        for (var year = start; year <= start + yearCount - 1; year++)
        {
            var startOfYear = new Date(year, 1, 1);
            var days = DateTime.IsLeapYear(year) ? 366 : 365;
            for (var day = 1; day <= days; day++)
            {
                yield return startOfYear.AddDays(day - 1);
            }
        }
    }

    public override IOrderedEnumerable<(Date date, string name)> ForYears(State state, int? startYear = null, int yearCount = 1) =>
        GetDates(startYear, yearCount)
            .Select(date => (date, "Holiday"))
            .OrderBy(_ => _.date);

    public override IOrderedEnumerable<(Date date, string name)> NationalForYears(int? startYear = null, int yearCount = 1) =>
        GetDates(startYear, yearCount)
            .Select(date => (date, "Holiday"))
            .OrderBy(_ => _.date);

    public override bool IsHoliday(Date date, State state) => true;

    public override bool IsHoliday(Date date, State state, [NotNullWhen(true)] out string? name)
    {
        name = "Holiday";
        return true;
    }

    public override bool IsActHoliday(Date date) => true;

    public override bool IsActHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "ACT Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForAct(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "ACT Holiday");

    public override bool IsNationalHoliday(Date date) => true;

    public override bool IsNationalHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "National Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForNational(int year) =>
        Holidays.ForNational(year);

    public override bool IsFederalGovernmentShutdown(Date date) => true;

    public override (Date start, Date end) GetFederalGovernmentShutdown(int startYear) => (Date.MinValue, Date.MaxValue);

    public override Task ExportToMarkdown(TextWriter writer, int? startYear = null, int yearCount = 5) =>  writer.WriteLineAsync("Holidays");

    public override Task<string> ExportToMarkdown(State state, int? startYear = null, int yearCount = 5) => Task.FromResult("Holidays");

    public override Task ExportToMarkdown(TextWriter writer, State state, int? startYear = null, int yearCount = 5) => writer.WriteLineAsync("Holidays");

    public override Task<string> ExportToMarkdown(int? startYear = null, int yearCount = 5) => Task.FromResult("Holidays");

    public override bool IsNswHoliday(Date date) => true;

    public override bool IsNswHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "NSW Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForNsw(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "NSW Holiday");

    public override bool IsNtHoliday(Date date) => true;

    public override bool IsNtHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "NT Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForNt(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "NT Holiday");

    public override bool IsQldHoliday(Date date) => true;

    public override bool IsQldHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "QLD Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForQld(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "Qld Holiday");

    public override bool IsSaHoliday(Date date) => true;

    public override bool IsSaHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "SA Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForSa(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "SA Holiday");

    public override bool IsTasHoliday(Date date) => true;

    public override bool IsTasHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "TAS Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForTas(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "Tas Holiday");

    public override bool IsVicHoliday(Date date) => true;

    public override bool IsVicHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "VIC Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForVic(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "Vic Holiday");

    public override bool IsWaHoliday(Date date) => true;

    public override bool IsWaHoliday(Date date, [NotNullWhen(true)] out string? name)
    {
        name = "WA Holiday";
        return true;
    }

    public override IReadOnlyDictionary<Date, string> ForWa(int year) =>
        GetDates(year, 1)
            .ToDictionary(_ => _, _ => "WA Holiday");
}