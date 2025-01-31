static class Extensions
{
    public static Date GetFirstMonday(Month month, int year)
    {
        var firstDay = new Date(year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilMonday);
    }

    public static Date GetSecondMonday(Month month, int year)
    {
        var firstDay = new Date(year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilMonday + 7);
    }

    public static Date GetFirstTuesday(Month month, int year)
    {
        var firstDay = new Date(year, (int)month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilTuesday = (9 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilTuesday);
    }

    public static Date GetLastFriday(Month month, int year)
    {
        var lastDayOfMonth = new Date(year, (int)month, DateTime.DaysInMonth(year, (int)month));
        var dayOfWeek = (int)lastDayOfMonth.DayOfWeek;
        var daysUntilFriday = dayOfWeek >= 5 ? dayOfWeek - 5 : dayOfWeek + 2;
        return lastDayOfMonth.AddDays(-daysUntilFriday);
    }
}