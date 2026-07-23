public class SittingFilterService(TimeProvider timeProvider)
{
    public static IReadOnlyList<int> GetAvailableYears(IReadOnlySet<Chamber> chambers) =>
        chambers
            .SelectMany(Parliament.CoveredYears)
            .Distinct()
            .Order()
            .ToList();

    // Prefers the current year when it has data, otherwise the latest available year, so the view opens
    // on something relevant rather than empty.
    public int? GetDefaultYear(IReadOnlySet<Chamber> chambers)
    {
        var years = GetAvailableYears(chambers);
        if (years.Count == 0)
        {
            return null;
        }

        var currentYear = Date.FromDateTime(timeProvider.GetLocalNow().DateTime).Year;
        return years.Contains(currentYear) ? currentYear : years[^1];
    }

    public IReadOnlyList<SittingViewModel> GetPeriods(IReadOnlySet<Chamber> chambers, IReadOnlySet<int> years)
    {
        if (chambers.Count == 0 ||
            years.Count == 0)
        {
            return [];
        }

        var today = Date.FromDateTime(timeProvider.GetLocalNow().DateTime);

        var list = new List<SittingViewModel>();
        foreach (var chamber in chambers.Order())
        {
            var covered = Parliament.CoveredYears(chamber);
            foreach (var year in years.Order())
            {
                if (!covered.Contains(year))
                {
                    continue;
                }

                foreach (var (start, end, name) in Parliament.GetSittingPeriods(chamber, year))
                {
                    list.Add(new(chamber, year, name, start, end, SittingChambers(start, end), GetTimeCategory(start, end, today)));
                }
            }
        }

        return list;
    }

    public IReadOnlyList<SittingViewModel> GetEstimates(IReadOnlySet<int> years)
    {
        if (years.Count == 0)
        {
            return [];
        }

        var today = Date.FromDateTime(timeProvider.GetLocalNow().DateTime);
        var covered = Parliament.CoveredYears(Chamber.Senate);

        var list = new List<SittingViewModel>();
        foreach (var year in years.Order())
        {
            if (!covered.Contains(year))
            {
                continue;
            }

            foreach (var (start, end, name) in Parliament.GetSenateEstimates(year))
            {
                list.Add(new(Chamber.Senate, year, name, start, end, [], GetTimeCategory(start, end, today)));
            }
        }

        return list;
    }

    // Which chambers sit for the whole period. A chamber only earns a badge when every one of the
    // period's days qualifies, so a partial overlap is not overstated. The period's own chamber always
    // qualifies; the other is there when the two sit together, which the published calendar marks with
    // an asterisk.
    static IReadOnlyList<Chamber> SittingChambers(Date start, Date end)
    {
        var list = new List<Chamber>();
        foreach (var chamber in Enum.GetValues<Chamber>())
        {
            if (SitsThroughout(chamber, start, end))
            {
                list.Add(chamber);
            }
        }

        return list;
    }

    static bool SitsThroughout(Chamber chamber, Date start, Date end)
    {
        for (var date = start; date <= end; date = date.AddDays(1))
        {
            if (!date.IsSittingDay(chamber))
            {
                return false;
            }
        }

        return true;
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
