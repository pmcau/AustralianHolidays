namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, FrozenDictionary<Date, string>> actCache;

    /// <summary>
    ///  Determines if the date is a public holiday in the Australian Capital Territory.
    ///  Reference: https://www.cmtedd.act.gov.au/communication/holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsActHoliday(this Date date) =>
        ForAct(date.Year)
            .ContainsKey(date);

    /// <summary>
    ///  Determines if the date is a public holiday in the Australian Capital Territory.
    ///  Reference: https://www.cmtedd.act.gov.au/communication/holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsActHoliday(this Date date, [NotNullWhen(true)] out string? name) =>
        ForAct(date.Year)
            .TryGetValue(date, out name);

    /// <summary>
    /// Gets all public holidays for the Australian Capital Territory for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> ForAct(int year) =>
        actCache.GetOrAdd(
            year,
            year => BuildActHolidays(year)
                .ToFrozenDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildActHolidays(int year)
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
                yield return (new(year, January, 28), "Australia Day (additional)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, January, 27), "Australia Day (additional)");
            }
        }

        yield return (Extensions.GetSecondMonday(March, year), "Canberra Day");

        static Date GetReconciliationDay(int year)
        {
            var startDate = new Date(year, May, 27);
            var dayOfWeek = (int)startDate.DayOfWeek;
            var daysUntilMonday = (8 - dayOfWeek) % 7;
            return startDate.AddDays(daysUntilMonday);
        }

        yield return (GetReconciliationDay(year), "Reconciliation Day");

        var anzacDate = AnzacDayCalculator.GetAnzacDay(year);

        if (anzacDate.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, April, 27), "Anzac Day (additional)");
        }
        else
        {
            yield return (anzacDate, "Anzac Day");
        }

        var (easterFriday, easterSaturday, easterSunday, easterMonday) = EasterCalculator.ForYear(year);
        yield return (easterFriday, "Good Friday");
        yield return (easterSaturday, "Easter Saturday");
        yield return (easterSunday, "Easter Sunday");
        yield return (easterMonday, "Easter Monday");

        yield return MonarchBirthdayCalculator.GetMonarchBirthday(year);

        yield return (Extensions.GetFirstMonday(October, year), "Labour Day");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }
}