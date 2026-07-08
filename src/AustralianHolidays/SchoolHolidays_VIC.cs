namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in Victoria.
        ///  Reference: https://www.vic.gov.au/school-term-dates-and-holidays-victoria
        /// </summary>
        public bool IsVicSchoolHoliday() =>
            date.IsSchoolHoliday(VIC);

        /// <summary>
        ///  Determines whether the date is a government school holiday in Victoria.
        ///  Reference: https://www.vic.gov.au/school-term-dates-and-holidays-victoria
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsVicSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(VIC, out name);
    }

    // Victorian government school term dates. Term 1 start is the first day students attend; the
    // Minister-approved term start is a curriculum (pupil-free) day the day before, which is treated
    // here as part of the summer break. Individual schools may schedule additional pupil-free days.
    // Source: https://www.vic.gov.au/school-term-dates-and-holidays-victoria
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> vicTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 1, 29), new(2025, 4, 4)), (new(2025, 4, 22), new(2025, 7, 4)), (new(2025, 7, 21), new(2025, 9, 19)), (new(2025, 10, 6), new(2025, 12, 19))],
            [2026] = [(new(2026, 1, 28), new(2026, 4, 2)), (new(2026, 4, 20), new(2026, 6, 26)), (new(2026, 7, 13), new(2026, 9, 18)), (new(2026, 10, 5), new(2026, 12, 18))],
            [2027] = [(new(2027, 1, 28), new(2027, 3, 25)), (new(2027, 4, 12), new(2027, 6, 25)), (new(2027, 7, 12), new(2027, 9, 17)), (new(2027, 10, 4), new(2027, 12, 17))],
            [2028] = [(new(2028, 1, 28), new(2028, 3, 31)), (new(2028, 4, 18), new(2028, 6, 30)), (new(2028, 7, 17), new(2028, 9, 22)), (new(2028, 10, 9), new(2028, 12, 21))],
            [2029] = [(new(2029, 1, 30), new(2029, 3, 29)), (new(2029, 4, 16), new(2029, 6, 29)), (new(2029, 7, 16), new(2029, 9, 21)), (new(2029, 10, 8), new(2029, 12, 21))],
            [2030] = [(new(2030, 1, 30), new(2030, 4, 5)), (new(2030, 4, 23), new(2030, 6, 28)), (new(2030, 7, 15), new(2030, 9, 20)), (new(2030, 10, 7), new(2030, 12, 20))],
        };
}
