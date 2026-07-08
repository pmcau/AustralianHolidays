namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in Western Australia.
        ///  Reference: https://www.education.wa.edu.au/future-term-dates
        /// </summary>
        public bool IsWaSchoolHoliday() =>
            date.IsSchoolHoliday(WA);

        /// <summary>
        ///  Determines whether the date is a government school holiday in Western Australia.
        ///  Reference: https://www.education.wa.edu.au/future-term-dates
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsWaSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(WA, out name);
    }

    // Western Australian government school term dates (student first/last days).
    // Source: https://www.education.wa.edu.au/future-term-dates
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> waTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 2, 5), new(2025, 4, 11)), (new(2025, 4, 28), new(2025, 7, 4)), (new(2025, 7, 21), new(2025, 9, 26)), (new(2025, 10, 13), new(2025, 12, 18))],
            [2026] = [(new(2026, 2, 2), new(2026, 4, 2)), (new(2026, 4, 20), new(2026, 7, 3)), (new(2026, 7, 20), new(2026, 9, 25)), (new(2026, 10, 12), new(2026, 12, 17))],
            [2027] = [(new(2027, 2, 1), new(2027, 4, 9)), (new(2027, 4, 26), new(2027, 7, 2)), (new(2027, 7, 19), new(2027, 9, 24)), (new(2027, 10, 11), new(2027, 12, 16))],
            [2028] = [(new(2028, 2, 2), new(2028, 4, 7)), (new(2028, 4, 24), new(2028, 6, 30)), (new(2028, 7, 17), new(2028, 9, 22)), (new(2028, 10, 9), new(2028, 12, 14))],
            [2029] = [(new(2029, 1, 31), new(2029, 3, 29)), (new(2029, 4, 16), new(2029, 6, 29)), (new(2029, 7, 16), new(2029, 9, 21)), (new(2029, 10, 8), new(2029, 12, 19))],
        };
}
