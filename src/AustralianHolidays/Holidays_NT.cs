namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, FrozenDictionary<Date, string>> ntCache;

    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines if the date is a public holiday in the Northern Territory.
        ///  Reference: https://nt.gov.au/nt-public-holidays
        /// </summary>
        public bool IsNtHoliday() =>
            ForNt(date.Year)
                .ContainsKey(date);

        /// <summary>
        ///  Determines if the date is a public holiday in the Northern Territory.
        ///  Reference: https://nt.gov.au/nt-public-holidays
        /// </summary>
        /// <param name="name">The name of the holiday.</param>
        public bool IsNtHoliday([NotNullWhen(true)] out string? name) =>
            ForNt(date.Year)
                .TryGetValue(date, out name);
    }

    /// <summary>
    ///  Gets all public holidays for the Northern Territory for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> ForNt(int year) =>
        ntCache.GetOrAdd(
            year,
            year =>
                BuildNtHolidays(year)
                    .ToFrozenDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildNtHolidays(int year)
    {
        yield return (new(year, January, 1), "New Year's Day");

        var australiaDay = GetAustraliaDay(year);
        if (australiaDay.IsWeekday())
        {
            yield return (australiaDay, "Australia Day");
        }
        else
        {
            if (australiaDay.DayOfWeek == DayOfWeek.Saturday)
            {
                yield return (new(year, January, 28), "Australia Day (observed)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, January, 27), "Australia Day (observed)");
            }
        }

        yield return (Extensions.GetFirstMonday(May, year), "May Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        var anzacDate = AnzacDayCalculator.GetAnzacDay(year);
        yield return (anzacDate, "Anzac Day");
        if (anzacDate.DayOfWeek == DayOfWeek.Sunday)
        {
            yield return (new(year, April, 26), "Anzac Day (additional)");
        }

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(August, year), "Picnic Day");

        yield return (ChristmasCalculator.ChristmasEve(year), "Christmas Eve (partial day)");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }

        yield return (new(year, December, 31), "New Year's Eve (partial day)");
    }
}
