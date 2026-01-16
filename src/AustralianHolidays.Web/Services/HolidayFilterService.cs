namespace AustralianHolidays.Web.Services;

public class HolidayFilterService
{
    public static IReadOnlyList<HolidayViewModel> GetHolidays(IReadOnlySet<State> states, Date startDate, Date endDate)
    {
        var startYear = startDate.Year;
        var endYear = endDate.Year;
        var yearCount = endYear - startYear + 1;

        var holidays = new List<(Date date, State state, string name)>();

        foreach (var (date, holidayState, name) in Holidays.ForYears(startYear, yearCount))
        {
            if (date >= startDate && date <= endDate && (states.Count == 0 || states.Contains(holidayState)))
            {
                holidays.Add((date, holidayState, name));
            }
        }

        // Group by date and name, combining states
        return holidays
            .GroupBy(h => (h.date, h.name))
            .Select(g => new HolidayViewModel(
                g.Key.date,
                g.Key.name,
                g.Select(h => h.state).OrderBy(s => s).ToList()))
            .OrderBy(h => h.Date)
            .ThenBy(h => h.Name)
            .ToList();
    }

    public static (Date Start, Date End) GetDefaultDateRange()
    {
        var today = Date.FromDateTime(DateTime.Today);
        var start = today.AddDays(-7);
        var end = today.AddMonths(12);
        return (start, end);
    }

    public static (int StartYear, int YearCount) GetExportYearRange(Date startDate, Date endDate) =>
        (startDate.Year, endDate.Year - startDate.Year + 1);
}

public record HolidayViewModel(Date Date, string Name, IReadOnlyList<State> States)
{
    public string DayOfWeek => Date.DayOfWeek.ToString();

    public HolidayTimeCategory TimeCategory
    {
        get
        {
            var today = Date.FromDateTime(DateTime.Today);
            if (Date < today)
            {
                return HolidayTimeCategory.Past;
            }

            if (Date == today)
            {
                return HolidayTimeCategory.Today;
            }

            return HolidayTimeCategory.Future;
        }
    }
}

public enum HolidayTimeCategory
{
    Past,
    Today,
    Future
}
