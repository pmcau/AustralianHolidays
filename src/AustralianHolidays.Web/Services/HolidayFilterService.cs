namespace AustralianHolidays.Web.Services;

public class HolidayFilterService
{
    public static IReadOnlyList<HolidayViewModel> GetHolidays(IReadOnlySet<State> states, Date startDate, Date endDate)
    {
        var startYear = startDate.Year;
        var endYear = endDate.Year;
        var yearCount = endYear - startYear + 1;

        var holidays = new List<HolidayViewModel>();

        foreach (var (date, holidayState, name) in Holidays.ForYears(startYear, yearCount))
        {
            if (date >= startDate && date <= endDate && (states.Count == 0 || states.Contains(holidayState)))
            {
                holidays.Add(new(date, name, holidayState));
            }
        }

        return holidays
            .OrderBy(h => h.Date)
            .ThenBy(h => h.State)
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

public record HolidayViewModel(Date Date, string Name, State State)
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
