namespace AustralianHolidays;

/// <summary>
/// Sitting days for the federal Parliament of Australia.
/// <para>
/// Sitting dates are agreed to by each House rather than calculated, and are published a year at a
/// time by the Department of the Prime Minister and Cabinet. They cannot be derived, so they are
/// stored as data and are only available for a bounded range of years.
/// </para>
/// <para>
/// The published calendar is indicative. Extra sittings can be added, scheduled ones cancelled, and
/// the whole calendar is displaced when Parliament is dissolved for an election.
/// </para>
/// <para>
/// Source: https://www.pmc.gov.au/resources/parliamentary-sittings
/// </para>
/// </summary>
public static partial class Parliament
{
    static IReadOnlyDictionary<int, (Date start, Date end, string name)[]> SittingData(Chamber chamber) =>
        chamber switch
        {
            House => houseSittings,
            Senate => senateSittings,
            _ => throw new ArgumentOutOfRangeException(nameof(chamber), chamber, null)
        };

    /// <summary>
    /// Determines whether a date is a sitting day for a chamber.
    /// <para>
    /// Returns false for years outside the available data.
    /// </para>
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="chamber">The chamber to check.</param>
    public static bool IsSittingDay(this Date date, Chamber chamber) =>
        date.IsSittingDay(chamber, out _);

    /// <summary>
    /// Determines whether a date is a sitting day for a chamber, and gets the name of the sitting
    /// block it falls in (Autumn, Winter or Spring).
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="chamber">The chamber to check.</param>
    /// <param name="name">The name of the sitting block.</param>
    public static bool IsSittingDay(this Date date, Chamber chamber, [NotNullWhen(true)] out string? name)
    {
        name = null;

        if (!SittingData(chamber).TryGetValue(date.Year, out var periods))
        {
            return false;
        }

        foreach (var (start, end, block) in periods)
        {
            if (date >= start &&
                date <= end)
            {
                name = block;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether a date is a sitting day for either chamber.
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsSittingDay(this Date date) =>
        date.IsSittingDay(House) ||
        date.IsSittingDay(Senate);

    /// <summary>
    /// Determines whether a date is a sitting day for both chambers. These are the dates marked with an
    /// asterisk on the published calendar.
    /// <para>
    /// This is not a joint sitting in the sense of section 57 of the Constitution. It only means both
    /// chambers are scheduled to sit that day.
    /// </para>
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsBothChambersSittingDay(this Date date) =>
        date.IsSittingDay(House) &&
        date.IsSittingDay(Senate);

    /// <summary>
    /// Gets the sitting periods for a chamber and year, ordered by start date. Each period is one run of
    /// consecutive sitting days, named for the sitting block (Autumn, Winter or Spring) it belongs to.
    /// </summary>
    /// <param name="chamber">The chamber to get periods for.</param>
    /// <param name="year">The calendar year.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no sitting data is available for the year.</exception>
    public static IReadOnlyList<(Date start, Date end, string name)> GetSittingPeriods(Chamber chamber, int year) =>
        PeriodsForYear(chamber, year);

    /// <summary>
    /// Gets every individual sitting day for a chamber and year, in ascending order.
    /// </summary>
    /// <param name="chamber">The chamber to get days for.</param>
    /// <param name="year">The calendar year.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no sitting data is available for the year.</exception>
    public static IReadOnlyList<Date> GetSittingDays(Chamber chamber, int year)
    {
        List<Date> days = [];
        foreach (var (start, end, _) in PeriodsForYear(chamber, year))
        {
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                days.Add(date);
            }
        }

        return days;
    }

    /// <summary>
    /// Gets the years for which sitting data is available for a chamber, in ascending order.
    /// </summary>
    /// <param name="chamber">The chamber to get years for.</param>
    public static IReadOnlyList<int> CoveredYears(Chamber chamber) =>
        SittingData(chamber)
            .Keys
            .Order()
            .ToList();

    /// <summary>
    /// Determines whether a date falls within a Senate estimates hearing period.
    /// <para>
    /// Estimates are committee hearings, not sittings, so the Senate is not sitting on these days.
    /// Returns false for years outside the available data.
    /// </para>
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsSenateEstimatesDay(this Date date) =>
        date.IsSenateEstimatesDay(out _);

    /// <summary>
    /// Determines whether a date falls within a Senate estimates hearing period, and gets the name of
    /// that round (Additional, Budget or Supplementary Budget).
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the estimates round.</param>
    public static bool IsSenateEstimatesDay(this Date date, [NotNullWhen(true)] out string? name)
    {
        name = null;

        if (!senateEstimates.TryGetValue(date.Year, out var periods))
        {
            return false;
        }

        foreach (var (start, end, round) in periods)
        {
            if (date >= start &&
                date <= end)
            {
                name = round;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the Senate estimates hearing periods for a year, ordered by start date.
    /// </summary>
    /// <param name="year">The calendar year.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no estimates data is available for the year.</exception>
    public static IReadOnlyList<(Date start, Date end, string name)> GetSenateEstimates(int year)
    {
        if (senateEstimates.TryGetValue(year, out var periods))
        {
            return periods;
        }

        throw NoData("Senate estimates", year, senateEstimates.Keys);
    }

    static (Date start, Date end, string name)[] PeriodsForYear(Chamber chamber, int year)
    {
        var data = SittingData(chamber);
        if (data.TryGetValue(year, out var periods))
        {
            return periods;
        }

        throw NoData($"{chamber} sitting", year, data.Keys);
    }

    static ArgumentOutOfRangeException NoData(string what, int year, IEnumerable<int> covered)
    {
        var years = covered.Order().ToList();
        return new(
            nameof(year),
            year,
            $"No {what} data for {year}. Available years: {years[0]}-{years[^1]}.");
    }
}
