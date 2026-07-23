namespace AustralianHolidays;

public static partial class Parliament
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a Senate sitting day.
        ///  Reference: https://www.pmc.gov.au/resources/parliamentary-sittings
        /// </summary>
        public bool IsSenateSittingDay() =>
            date.IsSittingDay(Senate);

        /// <summary>
        ///  Determines whether the date is a Senate sitting day.
        ///  Reference: https://www.pmc.gov.au/resources/parliamentary-sittings
        /// </summary>
        /// <param name="name">The name of the sitting block.</param>
        public bool IsSenateSittingDay([NotNullWhen(true)] out string? name) =>
            date.IsSittingDay(Senate, out name);
    }

    // Senate sitting days, grouped into the Autumn, Winter and Spring sitting blocks. Estimates weeks
    // are deliberately absent: they are committee hearings rather than sittings, which is why the Senate
    // is idle on several weeks the House sits (see senateEstimates below).
    // Source: https://www.pmc.gov.au/resources/parliamentary-sittings
    static readonly IReadOnlyDictionary<int, (Date start, Date end, string name)[]> senateSittings =
        new Dictionary<int, (Date start, Date end, string name)[]>
        {
            // Issued 26 November 2025.
            [2026] =
            [
                // Not on the published calendar: Parliament was recalled after the December 2025 Bondi
                // attack to pass a condolence motion and hate speech and gun law changes.
                (new(2026, January, 19), new(2026, January, 20), "Recall"),
                (new(2026, February, 3), new(2026, February, 5), "Autumn"),
                (new(2026, March, 2), new(2026, March, 5), "Autumn"),
                (new(2026, March, 10), new(2026, March, 12), "Autumn"),
                (new(2026, March, 23), new(2026, March, 26), "Autumn"),
                (new(2026, March, 30), new(2026, April, 1), "Autumn"),
                (new(2026, May, 12), new(2026, May, 14), "Winter"),
                (new(2026, June, 22), new(2026, June, 25), "Winter"),
                (new(2026, June, 29), new(2026, July, 2), "Winter"),
                (new(2026, August, 11), new(2026, August, 13), "Spring"),
                (new(2026, August, 17), new(2026, August, 20), "Spring"),
                (new(2026, September, 7), new(2026, September, 10), "Spring"),
                (new(2026, September, 14), new(2026, September, 17), "Spring"),
                (new(2026, October, 12), new(2026, October, 15), "Spring"),
                (new(2026, November, 16), new(2026, November, 19), "Spring"),
                (new(2026, November, 23), new(2026, November, 26), "Spring"),
            ],
        };

    // Senate estimates hearing rounds.
    // Source: https://www.pmc.gov.au/resources/parliamentary-sittings
    static readonly IReadOnlyDictionary<int, (Date start, Date end, string name)[]> senateEstimates =
        new Dictionary<int, (Date start, Date end, string name)[]>
        {
            [2026] =
            [
                (new(2026, February, 9), new(2026, February, 12), "Additional"),
                (new(2026, May, 25), new(2026, May, 28), "Budget"),
                // The PM&C page prints this round as "22 to 5 June", which is a typo: 22 June is a
                // Senate sitting day and the range would run backwards. The Parliamentary Handbook
                // sitting calendar shows the round as 2 to 5 June, which is what is used here.
                // https://handbook.aph.gov.au/resources/sitting-calendars/2026
                (new(2026, June, 2), new(2026, June, 5), "Budget"),
                (new(2026, October, 26), new(2026, October, 29), "Supplementary Budget"),
            ],
        };
}
