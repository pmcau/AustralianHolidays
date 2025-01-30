namespace AustralianHolidays;

public static partial class Holidays
{
    static ConcurrentDictionary<int, Dictionary<Date, string>> actHolidays = new();

    /// <summary>
    ///  Determines if the date is a public holiday in the Australian Capital Territory.
    ///  Reference: https://www.cmtedd.act.gov.au/communication/holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    public static bool IsActHoliday(this Date date) =>
        IsActHoliday(date, out _);

    /// <summary>
    ///  Determines if the date is a public holiday in the Australian Capital Territory.
    ///  Reference: https://www.cmtedd.act.gov.au/communication/holidays
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="name">The name of the holiday.</param>
    public static bool IsActHoliday(this Date date, [NotNullWhen(true)] out string? name)
    {
        var holidays = GetActHolidays(date.Year);

        return holidays.TryGetValue(date, out name);
    }

    /// <summary>
    /// Gets all public holidays for the Australian Capital Territory for the specified year.
    /// </summary>
    public static IReadOnlyDictionary<Date, string> GetActHolidays(int year) =>
        actHolidays.GetOrAdd(
            year,
            year => BuildActHolidays(year).ToDictionary(_ => _.date, _ => _.name));

    static IEnumerable<(Date date, string name)> BuildActHolidays(int year)
    {
        yield return (new(year, (int) Month.January, 1), "New Year's Day");

        var australiaDay = GetAustraliaDay(year);
        if (australiaDay.IsWeekday())
        {
            yield return (australiaDay, "Australia Day");
        }
        else
        {
            if (australiaDay.DayOfWeek == DayOfWeek.Saturday)
            {
                yield return (new(year, (int) Month.January, 28), "Australia Day (additional)");
            }
            else if (australiaDay.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return (new(year, (int) Month.January, 27), "Australia Day (additional)");
            }
        }

        yield return (Extensions.GetSecondMonday(Month.March, year), "Canberra Day");

        Date GetReconciliationDay()
        {
            var startDate = new Date(year, (int) Month.May, 27);
            var dayOfWeek = (int)startDate.DayOfWeek;
            var daysUntilMonday = (8 - dayOfWeek) % 7;
            return startDate.AddDays(daysUntilMonday);
        }

        yield return (GetReconciliationDay(), "Reconciliation Day");

        var anzacDate = AnzacDayCalculator.GetAnzacDay(year);

        if (anzacDate.DayOfWeek == DayOfWeek.Saturday)
        {
            yield return (new(year, (int) Month.April, 27), "Anzac Day (additional)");
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

        yield return (Extensions.GetFirstMonday(Month.October, year), "Labour Day");

        foreach (var date in ChristmasCalculator.Get(year))
        {
            yield return date;
        }
    }

    private static Date GetAustraliaDay(int year) => new(year, (int) Month.January, 26);
}