namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in New South Wales (Eastern Division).
        ///  Reference: https://education.nsw.gov.au/schooling/calendars
        /// </summary>
        public bool IsNswSchoolHoliday() =>
            date.IsSchoolHoliday(NSW);

        /// <summary>
        ///  Determines whether the date is a government school holiday in New South Wales (Eastern Division).
        ///  Reference: https://education.nsw.gov.au/schooling/calendars
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsNswSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(NSW, out name);
    }

    // New South Wales government school term dates, Eastern Division (student first/last days; the
    // School Development Days before Term 1 count as part of the summer break). Western Division
    // schools start Term 1 about a week later; Terms 2-4 are identical.
    // Coverage stops at 2027 because NSW only publishes the student first-day breakdown through 2027;
    // 2028+ terms are gazetted only as staff-inclusive start dates, which would misreport late January.
    // Source: https://education.nsw.gov.au/schooling/calendars
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> nswTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 2, 6), new(2025, 4, 11)), (new(2025, 4, 30), new(2025, 7, 4)), (new(2025, 7, 22), new(2025, 9, 26)), (new(2025, 10, 14), new(2025, 12, 19))],
            [2026] = [(new(2026, 2, 2), new(2026, 4, 2)), (new(2026, 4, 22), new(2026, 7, 3)), (new(2026, 7, 21), new(2026, 9, 25)), (new(2026, 10, 13), new(2026, 12, 17))],
            [2027] = [(new(2027, 2, 3), new(2027, 4, 9)), (new(2027, 4, 29), new(2027, 7, 2)), (new(2027, 7, 20), new(2027, 9, 24)), (new(2027, 10, 12), new(2027, 12, 20))],
        };
}
