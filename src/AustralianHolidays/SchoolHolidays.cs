namespace AustralianHolidays;

/// <summary>
/// School holiday and term dates for Australian government schools.
/// <para>
/// Unlike public holidays, school term dates are set administratively by each state and territory
/// education department and published year by year. They cannot be calculated, so they are stored as
/// data sourced from official calendars and are only available for a bounded range of years.
/// </para>
/// <para>
/// Dates are for government (public) schools. Non-government (Catholic/independent) schools and, in
/// New South Wales, the Western Division "late-start" schools can differ.
/// </para>
/// </summary>
public static partial class SchoolHolidays
{
    static IReadOnlyDictionary<int, (Date start, Date end)[]> TermData(State state) =>
        state switch
        {
            ACT => actTerms,
            NSW => nswTerms,
            NT => ntTerms,
            QLD => qldTerms,
            SA => saTerms,
            TAS => tasTerms,
            VIC => vicTerms,
            WA => waTerms,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

    /// <summary>
    /// Gets the four school terms for a state and year.
    /// </summary>
    /// <param name="state">The state to get terms for.</param>
    /// <param name="year">The calendar year.</param>
    /// <returns>The four terms, each with its number (1-4), start (first day) and end (last day).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no term data is available for the year.</exception>
    public static IReadOnlyList<(int number, Date start, Date end)> GetTerms(State state, int year)
    {
        var terms = TermsForYear(state, year);

        var result = new (int number, Date start, Date end)[terms.Length];
        for (var index = 0; index < terms.Length; index++)
        {
            result[index] = (index + 1, terms[index].start, terms[index].end);
        }

        return result;
    }

    /// <summary>
    /// Gets the school holiday (vacation) periods for a state and year.
    /// <para>
    /// A year owns four periods: the summer break leading into Term 1, then the autumn, winter and
    /// spring breaks between terms. The break after Term 4 belongs to the following year's summer
    /// period, so it is not repeated here. When the previous year is outside the available data, the
    /// summer period is clamped to 1 January of the requested year.
    /// </para>
    /// </summary>
    /// <param name="state">The state to get holidays for.</param>
    /// <param name="year">The calendar year.</param>
    /// <returns>The vacation periods, each with a start, end and name (Summer, Autumn, Winter, Spring).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no term data is available for the year.</exception>
    public static IReadOnlyList<(Date start, Date end, string name)> GetHolidays(State state, int year)
    {
        var terms = TermsForYear(state, year);
        var data = TermData(state);

        List<(Date start, Date end, string name)> holidays = [];

        // Summer: the break leading into Term 1. It starts the day after the previous year's Term 4,
        // spanning the year boundary. When the previous year has no data, clamp to 1 January.
        var summerStart = data.TryGetValue(year - 1, out var previous)
            ? previous[3].end.AddDays(1)
            : new(year, 1, 1);
        var summerEnd = terms[0].start.AddDays(-1);
        if (summerStart <= summerEnd)
        {
            holidays.Add((summerStart, summerEnd, "Summer"));
        }

        holidays.Add((terms[0].end.AddDays(1), terms[1].start.AddDays(-1), "Autumn"));
        holidays.Add((terms[1].end.AddDays(1), terms[2].start.AddDays(-1), "Winter"));
        holidays.Add((terms[2].end.AddDays(1), terms[3].start.AddDays(-1), "Spring"));

        return holidays;
    }

    /// <summary>
    /// Determines whether a date falls within a government school holiday period for a state.
    /// <para>
    /// Weekends and public holidays that fall during a school term are not school holidays. Returns
    /// false for years outside the available data.
    /// </para>
    /// </summary>
    public static bool IsSchoolHoliday(this Date date, State state) =>
        date.IsSchoolHoliday(state, out _);

    /// <summary>
    /// Determines whether a date falls within a government school holiday period for a state, and gets
    /// the name of that period (Summer, Autumn, Winter or Spring).
    /// </summary>
    public static bool IsSchoolHoliday(this Date date, State state, [NotNullWhen(true)] out string? name)
    {
        name = null;

        if (!TermData(state).TryGetValue(date.Year, out var terms))
        {
            return false;
        }

        // Any date within one of the four terms is school time, not a holiday.
        foreach (var (start, end) in terms)
        {
            if (date >= start &&
                date <= end)
            {
                return false;
            }
        }

        // Every other day of a covered year is a vacation; name it by where it sits relative to terms.
        name = VacationName(date, terms);
        return true;
    }

    // Assumes the date is not inside any term (checked by the caller). The remaining days bucket into
    // the gap before Term 1 (summer), the gaps between terms (autumn/winter/spring), or after Term 4
    // (next summer).
    static string VacationName(Date date, (Date start, Date end)[] terms)
    {
        if (date < terms[0].start)
        {
            return "Summer";
        }

        if (date < terms[1].start)
        {
            return "Autumn";
        }

        if (date < terms[2].start)
        {
            return "Winter";
        }

        if (date < terms[3].start)
        {
            return "Spring";
        }

        return "Summer";
    }

    /// <summary>
    /// Gets the years for which school term data is available for a state, in ascending order.
    /// </summary>
    public static IReadOnlyList<int> CoveredYears(State state) =>
        TermData(state)
            .Keys
            .Order()
            .ToList();

    static (Date start, Date end)[] TermsForYear(State state, int year)
    {
        var data = TermData(state);
        if (data.TryGetValue(year, out var terms))
        {
            return terms;
        }

        var years = data.Keys.Order().ToList();
        throw new ArgumentOutOfRangeException(
            nameof(year),
            year,
            $"No school term data for {state} in {year}. Available years: {years[0]}-{years[^1]}.");
    }
}
