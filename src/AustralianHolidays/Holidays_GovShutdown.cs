namespace AustralianHolidays;

public static partial class Holidays
{
    public static bool IsFederalGovernmentShutdown(this Date date)
    {
        // A shutdown spans late December of one year into early January of the next. For a date in
        // January the relevant period started at the previous year's Christmas, so anchor on that
        // year rather than the date's own year (otherwise January dates are never matched).
        var startYear = date.Month == 12 ? date.Year : date.Year - 1;

        var (start, end) = GetFederalGovernmentShutdown(startYear);

        return date >= start &&
               date <= end;
    }

    public static (Date start, Date end) GetFederalGovernmentShutdown(int startYear)
    {
        var christmas = new Date(startYear, 12, 25);

        var newYearsHoliday = GetNewYearsHoliday(startYear + 1);

        return (christmas, newYearsHoliday);
    }
}