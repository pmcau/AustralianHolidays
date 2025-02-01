static class Extensions
{
    public static Date GetFirstMonday(int month, int year)
    {
        var firstDay = new Date(year, month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilMonday);
    }

    public static Date GetSecondMonday(int month, int year)
    {
        var firstDay = new Date(year, month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilMonday = (8 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilMonday + 7);
    }

    public static Date GetFirstTuesday(int month, int year)
    {
        var firstDay = new Date(year, month, 1);
        var dayOfWeek = (int)firstDay.DayOfWeek;
        var daysUntilTuesday = (9 - dayOfWeek) % 7;
        return firstDay.AddDays(daysUntilTuesday);
    }

    public static Date GetLastFriday(int month, int year)
    {
        var lastDayOfMonth = new Date(year, month, DateTime.DaysInMonth(year, month));
        var dayOfWeek = (int)lastDayOfMonth.DayOfWeek;
        var daysUntilFriday = dayOfWeek >= 5 ? dayOfWeek - 5 : dayOfWeek + 2;
        return lastDayOfMonth.AddDays(-daysUntilFriday);
    }
}