namespace AustralianHolidays.Web.Services;

public class HolidayFilterService
{
    public static IReadOnlyList<HolidayViewModel> GetHolidays(State? state, DateOnly startDate, DateOnly endDate)
    {
        var startYear = startDate.Year;
        var endYear = endDate.Year;
        var yearCount = endYear - startYear + 1;

        var holidays = new List<HolidayViewModel>();

        if (state.HasValue)
        {
            // Get holidays for a specific state
            foreach (var (date, name) in Holidays.ForYears(state.Value, startYear, yearCount))
            {
                if (date >= startDate && date <= endDate)
                {
                    holidays.Add(new HolidayViewModel(date, name, state.Value));
                }
            }
        }
        else
        {
            // Get holidays for all states
            foreach (var (date, holidayState, name) in Holidays.ForYears(startYear, yearCount))
            {
                if (date >= startDate && date <= endDate)
                {
                    holidays.Add(new HolidayViewModel(date, name, holidayState));
                }
            }
        }

        return holidays
            .OrderBy(h => h.Date)
            .ThenBy(h => h.State)
            .ToList();
    }

    public static (DateOnly Start, DateOnly End) GetDefaultDateRange()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var start = today.AddDays(-7);
        var end = today.AddMonths(12);
        return (start, end);
    }

    public static (int StartYear, int YearCount) GetExportYearRange(DateOnly startDate, DateOnly endDate) =>
        (startDate.Year, endDate.Year - startDate.Year + 1);
}

public record HolidayViewModel(DateOnly Date, string Name, State State)
{
    public string DayOfWeek => Date.DayOfWeek.ToString();

    public HolidayTimeCategory TimeCategory
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
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
