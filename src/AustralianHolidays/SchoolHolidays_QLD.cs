namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in Queensland.
        ///  Reference: https://education.qld.gov.au/about-us/calendar/future-dates
        /// </summary>
        public bool IsQldSchoolHoliday() =>
            date.IsSchoolHoliday(QLD);

        /// <summary>
        ///  Determines whether the date is a government school holiday in Queensland.
        ///  Reference: https://education.qld.gov.au/about-us/calendar/future-dates
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsQldSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(QLD, out name);
    }

    // Queensland government school term dates (student first/last days).
    // Source: https://education.qld.gov.au/about-us/calendar/future-dates
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> qldTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 1, 28), new(2025, 4, 4)), (new(2025, 4, 22), new(2025, 6, 27)), (new(2025, 7, 14), new(2025, 9, 19)), (new(2025, 10, 7), new(2025, 12, 12))],
            [2026] = [(new(2026, 1, 27), new(2026, 4, 2)), (new(2026, 4, 20), new(2026, 6, 26)), (new(2026, 7, 13), new(2026, 9, 18)), (new(2026, 10, 6), new(2026, 12, 11))],
            [2027] = [(new(2027, 1, 27), new(2027, 3, 25)), (new(2027, 4, 12), new(2027, 6, 25)), (new(2027, 7, 12), new(2027, 9, 17)), (new(2027, 10, 5), new(2027, 12, 10))],
            [2028] = [(new(2028, 1, 24), new(2028, 3, 31)), (new(2028, 4, 18), new(2028, 6, 23)), (new(2028, 7, 10), new(2028, 9, 15)), (new(2028, 10, 3), new(2028, 12, 8))],
            [2029] = [(new(2029, 1, 22), new(2029, 3, 29)), (new(2029, 4, 16), new(2029, 6, 22)), (new(2029, 7, 9), new(2029, 9, 14)), (new(2029, 10, 2), new(2029, 12, 7))],
        };
}
