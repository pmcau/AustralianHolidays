namespace AustralianHolidays;

public static partial class Holidays
{
    public static bool IsFederalGovernmentShutdown(this Date date)
    {
        var christmas = new Date(date.Year, 12, 25);

        var newYearsHoliday = GetNewYearsHoliday(date.Year + 1);

        return date >= christmas &&
               date <= newYearsHoliday;
    }

    public static (Date start, Date end) GetFederalGovernmentShutdown(int startYear)
    {
        var christmas = new Date(startYear, 12, 25);

        var newYearsHoliday = GetNewYearsHoliday(startYear + 1);

        return (christmas, newYearsHoliday);
    }
}