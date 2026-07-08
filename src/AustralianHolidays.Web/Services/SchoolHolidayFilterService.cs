public class SchoolHolidayFilterService(TimeProvider timeProvider)
{
    public static IReadOnlyList<int> GetAvailableYears(IReadOnlySet<State> states) =>
        states
            .SelectMany(SchoolHolidays.CoveredYears)
            .Distinct()
            .Order()
            .ToList();

    // Prefers the current year when it has data, otherwise the latest available year, so the school
    // view opens on something relevant rather than empty.
    public int? GetDefaultYear(IReadOnlySet<State> states)
    {
        var years = GetAvailableYears(states);
        if (years.Count == 0)
        {
            return null;
        }

        var currentYear = Date.FromDateTime(timeProvider.GetLocalNow().DateTime).Year;
        return years.Contains(currentYear) ? currentYear : years[^1];
    }

    public IReadOnlyList<SchoolHolidayViewModel> GetHolidays(IReadOnlySet<State> states, IReadOnlySet<int> years)
    {
        if (states.Count == 0 ||
            years.Count == 0)
        {
            return [];
        }

        var today = Date.FromDateTime(timeProvider.GetLocalNow().DateTime);

        var list = new List<SchoolHolidayViewModel>();
        foreach (var state in states.Order())
        {
            var covered = SchoolHolidays.CoveredYears(state);
            foreach (var year in years.Order())
            {
                if (!covered.Contains(year))
                {
                    continue;
                }

                foreach (var (start, end, name) in SchoolHolidays.GetHolidays(state, year))
                {
                    list.Add(new(state, year, name, start, end, GetTimeCategory(start, end, today)));
                }
            }
        }

        return list;
    }

    static HolidayTimeCategory GetTimeCategory(Date start, Date end, Date today)
    {
        if (end < today)
        {
            return HolidayTimeCategory.Past;
        }

        if (start <= today)
        {
            return HolidayTimeCategory.Today;
        }

        return HolidayTimeCategory.Future;
    }
}
