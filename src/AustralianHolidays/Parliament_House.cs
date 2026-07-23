namespace AustralianHolidays;

public static partial class Parliament
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a House of Representatives sitting day.
        ///  Reference: https://www.pmc.gov.au/resources/parliamentary-sittings
        /// </summary>
        public bool IsHouseSittingDay() =>
            date.IsSittingDay(House);

        /// <summary>
        ///  Determines whether the date is a House of Representatives sitting day.
        ///  Reference: https://www.pmc.gov.au/resources/parliamentary-sittings
        /// </summary>
        /// <param name="name">The name of the sitting block.</param>
        public bool IsHouseSittingDay([NotNullWhen(true)] out string? name) =>
            date.IsSittingDay(House, out name);
    }

    // House of Representatives sitting days, grouped into the Autumn, Winter and Spring sitting blocks.
    // Each entry is one run of consecutive sitting days; every run falls Monday to Friday, so a plain
    // range check never needs to exclude a weekend.
    // Source: https://www.pmc.gov.au/resources/parliamentary-sittings
    static readonly IReadOnlyDictionary<int, (Date start, Date end, string name)[]> houseSittings =
        new Dictionary<int, (Date start, Date end, string name)[]>
        {
            // Issued 26 November 2025. The Budget is on 12 May 2026.
            [2026] =
            [
                // Not on the published calendar: Parliament was recalled after the December 2025 Bondi
                // attack to pass a condolence motion and hate speech and gun law changes.
                (new(2026, January, 19), new(2026, January, 20), "Recall"),
                (new(2026, February, 3), new(2026, February, 5), "Autumn"),
                (new(2026, February, 9), new(2026, February, 12), "Autumn"),
                (new(2026, March, 2), new(2026, March, 5), "Autumn"),
                (new(2026, March, 10), new(2026, March, 12), "Autumn"),
                (new(2026, March, 23), new(2026, March, 26), "Autumn"),
                (new(2026, March, 30), new(2026, April, 1), "Autumn"),
                (new(2026, May, 12), new(2026, May, 14), "Winter"),
                (new(2026, May, 25), new(2026, May, 28), "Winter"),
                (new(2026, June, 2), new(2026, June, 4), "Winter"),
                (new(2026, June, 22), new(2026, June, 25), "Winter"),
                (new(2026, June, 29), new(2026, July, 2), "Winter"),
                (new(2026, August, 11), new(2026, August, 13), "Spring"),
                (new(2026, August, 17), new(2026, August, 20), "Spring"),
                (new(2026, September, 7), new(2026, September, 10), "Spring"),
                (new(2026, September, 14), new(2026, September, 17), "Spring"),
                (new(2026, October, 12), new(2026, October, 15), "Spring"),
                (new(2026, October, 26), new(2026, October, 29), "Spring"),
                (new(2026, November, 23), new(2026, November, 26), "Spring"),
            ],
        };
}
