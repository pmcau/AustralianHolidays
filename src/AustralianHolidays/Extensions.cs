namespace AustralianHolidays;

static class Extensions
{
    public static bool IsFirstMonday(this Date date, Month month)
    {
        var dateYear = date.Year;
        var firstDay = new Date(dateYear, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        var firstMonday = firstDay.AddDays(daysUntilMonday);
        return date == firstMonday;
    }

    public static bool IsSecondMonday(this Date date, Month month)
    {
        var firstDay = new Date(date.Year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        var secondMonday = firstDay.AddDays(daysUntilMonday + 7);
        return date == secondMonday;
    }
}