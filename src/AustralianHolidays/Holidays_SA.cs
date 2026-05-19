namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, FrozenDictionary<Date, string>> saCache;

    /// <param name="date">The date to check.</param>
    extension(Date date)
    {
        /// <summary>
        ///  Determines if the date is a public holiday in South Australia.
        ///  Reference: https://www.safework.sa.gov.au/resources/public-holidays
        /// </summary>
        public bool IsSaHoliday() =>
            ForSa(date.Year)
                .ContainsKey(date);

        /// <summary>
        ///  Determines if the date is a public holiday in South Australia.
        ///  Reference: https://www.safework.sa.gov.au/resources/public-holidays
        /// </summary>
        /// <param name="name">The name of the holiday.</param>
        public bool IsSaHoliday([NotNullWhen(true)] out string? name) =>
            ForSa(date.Year)
                .TryGetValue(date, out name);
    }

    /// <summary>
    ///  Gets all public holidays for South Australia for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> ForSa(int year) =>
        saCache.GetOrAdd(
            year,
            year =>
                BuildSaHolidays(year)
                    .ToFrozenDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildSaHolidays(int year)
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

        yield return (Extensions.GetSecondMonday(March, year), "Adelaide Cup Day");

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return (AnzacDayCalculator.GetAnzacDay(year), "Anzac Day");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(October, year), "Labour Day");

        yield return (ChristmasCalculator.ChristmasEve(year), "Christmas Eve (partial day)");
        yield return (ChristmasCalculator.ChristmasDay(year), "Christmas Day");

        var proclamationDay = new Date(year, December, 26);
        if (proclamationDay.IsWeekday())
        {
            yield return (proclamationDay, "Proclamation and Boxing Day");
        }
        else if (proclamationDay.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (proclamationDay, "Proclamation Day");
            yield return (proclamationDay.AddDays(2), "Proclamation Day (additional)");
        }
        else if (proclamationDay.DayOfWeek == DayOfWeek.Sunday)
        {
            yield return (proclamationDay, "Proclamation Day");
            yield return (proclamationDay.AddDays(1), "Proclamation Day (additional)");
        }

        yield return (new(year, December, 31), "New Year's Eve (partial day)");
    }
}
