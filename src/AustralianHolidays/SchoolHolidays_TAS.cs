namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in Tasmania.
        ///  Reference: https://www.decyp.tas.gov.au/learning/term-dates/
        /// </summary>
        public bool IsTasSchoolHoliday() =>
            date.IsSchoolHoliday(TAS);

        /// <summary>
        ///  Determines whether the date is a government school holiday in Tasmania.
        ///  Reference: https://www.decyp.tas.gov.au/learning/term-dates/
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsTasSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(TAS, out name);
    }

    // Tasmanian government school term dates (student first/last days).
    // Coverage stops at 2027 because that is the furthest year DECYP has published (checked
    // August 2026); 2028 term dates are not yet available from any official source.
    // Source: https://www.decyp.tas.gov.au/learning/term-dates/
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> tasTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 2, 6), new(2025, 4, 11)), (new(2025, 4, 28), new(2025, 7, 4)), (new(2025, 7, 21), new(2025, 9, 26)), (new(2025, 10, 13), new(2025, 12, 18))],
            [2026] = [(new(2026, 2, 5), new(2026, 4, 17)), (new(2026, 5, 4), new(2026, 7, 10)), (new(2026, 7, 27), new(2026, 10, 2)), (new(2026, 10, 19), new(2026, 12, 18))],
            [2027] = [(new(2027, 2, 4), new(2027, 4, 9)), (new(2027, 4, 26), new(2027, 7, 2)), (new(2027, 7, 19), new(2027, 9, 24)), (new(2027, 10, 11), new(2027, 12, 16))],
        };
}
