namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in South Australia.
        ///  Reference: https://www.education.sa.gov.au/students/term-dates-south-australian-state-schools
        /// </summary>
        public bool IsSaSchoolHoliday() =>
            date.IsSchoolHoliday(SA);

        /// <summary>
        ///  Determines whether the date is a government school holiday in South Australia.
        ///  Reference: https://www.education.sa.gov.au/students/term-dates-south-australian-state-schools
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsSaSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(SA, out name);
    }

    // South Australian government school term dates.
    // Source: https://www.education.sa.gov.au/students/term-dates-south-australian-state-schools
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> saTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 1, 28), new(2025, 4, 11)), (new(2025, 4, 28), new(2025, 7, 4)), (new(2025, 7, 21), new(2025, 9, 26)), (new(2025, 10, 13), new(2025, 12, 12))],
            [2026] = [(new(2026, 1, 27), new(2026, 4, 10)), (new(2026, 4, 27), new(2026, 7, 3)), (new(2026, 7, 20), new(2026, 9, 25)), (new(2026, 10, 12), new(2026, 12, 11))],
            [2027] = [(new(2027, 1, 27), new(2027, 4, 9)), (new(2027, 4, 26), new(2027, 7, 2)), (new(2027, 7, 19), new(2027, 9, 24)), (new(2027, 10, 11), new(2027, 12, 10))],
            [2028] = [(new(2028, 1, 31), new(2028, 4, 13)), (new(2028, 5, 1), new(2028, 7, 7)), (new(2028, 7, 24), new(2028, 9, 29)), (new(2028, 10, 16), new(2028, 12, 15))],
            [2029] = [(new(2029, 1, 29), new(2029, 4, 13)), (new(2029, 4, 30), new(2029, 7, 6)), (new(2029, 7, 23), new(2029, 9, 28)), (new(2029, 10, 15), new(2029, 12, 14))],
            [2030] = [(new(2030, 1, 29), new(2030, 4, 12)), (new(2030, 4, 29), new(2030, 7, 5)), (new(2030, 7, 22), new(2030, 9, 27)), (new(2030, 10, 14), new(2030, 12, 13))],
        };
}
