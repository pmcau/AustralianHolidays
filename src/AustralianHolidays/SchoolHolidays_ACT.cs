namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines whether the date is a government school holiday in the Australian Capital Territory.
        ///  Reference: https://www.act.gov.au/living-in-the-act/public-holidays-school-terms-and-daylight-saving
        /// </summary>
        public bool IsActSchoolHoliday() =>
            date.IsSchoolHoliday(ACT);

        /// <summary>
        ///  Determines whether the date is a government school holiday in the Australian Capital Territory.
        ///  Reference: https://www.act.gov.au/living-in-the-act/public-holidays-school-terms-and-daylight-saving
        /// </summary>
        /// <param name="name">The name of the school holiday period.</param>
        public bool IsActSchoolHoliday([NotNullWhen(true)] out string? name) =>
            date.IsSchoolHoliday(ACT, out name);
    }

    // Australian Capital Territory government school term dates. Start = first day students attend
    // (continuing students return the next school day); end = last student day.
    // Source: https://www.act.gov.au/living-in-the-act/public-holidays-school-terms-and-daylight-saving
    static readonly IReadOnlyDictionary<int, (Date start, Date end)[]> actTerms =
        new Dictionary<int, (Date start, Date end)[]>
        {
            [2025] = [(new(2025, 2, 3), new(2025, 4, 11)), (new(2025, 4, 29), new(2025, 7, 4)), (new(2025, 7, 22), new(2025, 9, 26)), (new(2025, 10, 14), new(2025, 12, 18))],
            [2026] = [(new(2026, 1, 30), new(2026, 4, 2)), (new(2026, 4, 21), new(2026, 7, 3)), (new(2026, 7, 21), new(2026, 9, 25)), (new(2026, 10, 13), new(2026, 12, 18))],
            [2027] = [(new(2027, 2, 1), new(2027, 4, 9)), (new(2027, 4, 28), new(2027, 7, 2)), (new(2027, 7, 20), new(2027, 9, 24)), (new(2027, 10, 12), new(2027, 12, 17))],
            [2028] = [(new(2028, 2, 8), new(2028, 4, 7)), (new(2028, 4, 26), new(2028, 7, 7)), (new(2028, 7, 25), new(2028, 9, 29)), (new(2028, 10, 17), new(2028, 12, 22))],
            [2029] = [(new(2029, 2, 6), new(2029, 4, 13)), (new(2029, 5, 1), new(2029, 7, 6)), (new(2029, 7, 24), new(2029, 9, 28)), (new(2029, 10, 16), new(2029, 12, 21))],
            [2030] = [(new(2030, 2, 5), new(2030, 4, 12)), (new(2030, 4, 30), new(2030, 7, 5)), (new(2030, 7, 23), new(2030, 9, 27)), (new(2030, 10, 15), new(2030, 12, 20))],
        };
}
