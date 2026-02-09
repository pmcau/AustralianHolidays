public class HolidayFilterService(TimeProvider timeProvider)
{
    public IReadOnlyList<HolidayViewModel> GetHolidays(IReadOnlySet<State> states, Date startDate, Date endDate)
    {
        if (states.Count == 0)
        {
            return [];
        }

        var startYear = startDate.Year;
        var endYear = endDate.Year;
        var yearCount = endYear - startYear + 1;

        var holidays = new List<(Date date, State state, string name)>();

        foreach (var (date, holidayState, name) in Holidays.ForYears(startYear, yearCount))
        {
            if (date >= startDate && date <= endDate && states.Contains(holidayState))
            {
                holidays.Add((date, holidayState, name));
            }
        }

        var today = Date.FromDateTime(timeProvider.GetLocalNow().DateTime);

        // Group by date and name, combining states
        return holidays
            .GroupBy(_ => (_.date, _.name))
            .Select(_ =>
                new HolidayViewModel(
                _.Key.date,
                _.Key.name,
                _.Select(_=> _.state).OrderBy(_ => _).ToList(),
                GetTimeCategory(_.Key.date, today)))
            .OrderBy(_ => _.Date)
            .ThenBy(_ => _.Name)
            .ToList();
    }

    public (Date Start, Date End) GetDefaultDateRange()
    {
        var today = Date.FromDateTime(timeProvider.GetLocalNow().DateTime);
        var start = today.AddDays(-7);
        var end = today.AddMonths(12);
        return (start, end);
    }

    public static (int StartYear, int YearCount) GetExportYearRange(Date startDate, Date endDate) =>
        (startDate.Year, endDate.Year - startDate.Year + 1);

    static HolidayTimeCategory GetTimeCategory(Date date, Date today)
    {
        if (date < today)
        {
            return HolidayTimeCategory.Past;
        }

        if (date == today)
        {
            return HolidayTimeCategory.Today;
        }

        return HolidayTimeCategory.Future;
    }
}
