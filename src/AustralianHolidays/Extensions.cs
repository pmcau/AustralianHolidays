static class Extensions
{
    public static bool IsFirstMonday(this Date date, Month month)
    {
        var year = date.Year;
        var firstMonday = GetFirstMonday(month, year);
        return date == firstMonday;
    }

    public static Date GetFirstMonday(Month month, int year)
    {
        var firstDay = new Date(year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilMonday);
    }

    public static bool IsSecondMonday(this Date date, Month month)
    {
        var year = date.Year;
        var secondMonday = GetSecondMonday(month, year);
        return date == secondMonday;
    }

    public static Date GetSecondMonday(Month month, int year)
    {
        var firstDay = new Date(year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        var secondMonday = firstDay.AddDays(daysUntilMonday + 7);
        return secondMonday;
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