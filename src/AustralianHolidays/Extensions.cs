namespace AustralianHolidays;

static class Extensions
{
    public static bool IsLastMondayInMonth(this Date date, Month month)
    {
        var lastDayOfMonth = new Date(date.Year, (int)month, DateTime.DaysInMonth(date.Year, (int)month));
        var dayOfWeek = (int)lastDayOfMonth.DayOfWeek;
        var daysUntilMonday = dayOfWeek >= 1 ? dayOfWeek - 1 : 6;
        var lastMonday = lastDayOfMonth.AddDays(-daysUntilMonday);
        return date == lastMonday;
    }
    public static bool IsFirstMonday(this Date date, Month month)
    {
        var firstDay = new Date(date.Year, (int)month, 1);
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

    public static bool IsFirstTuesday(this Date date, Month month)
    {
        var firstDay = new Date(date.Year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilTuesday = (9 - dayOfWeek) % 7;
        var firstTuesday = firstDay.AddDays(daysUntilTuesday);
        return date == firstTuesday;
    }

    public static bool IsLastFridayInMonth(this Date date, Month month)
    {
        var lastDayOfMonth = new Date(date.Year, (int)month, DateTime.DaysInMonth(date.Year, (int)month));
        var dayOfWeek = (int)lastDayOfMonth.DayOfWeek;
        var daysUntilFriday = dayOfWeek >= 5 ? dayOfWeek - 5 : dayOfWeek + 2;
        var lastFriday = lastDayOfMonth.AddDays(-daysUntilFriday);
        return date == lastFriday;
    }
}