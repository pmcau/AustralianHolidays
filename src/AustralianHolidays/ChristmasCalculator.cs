namespace AustralianHolidays;

public static class ChristmasCalculator
{
    public static Date ChristmasDay(int year) =>
        new(year, 12, 25);

    public static Date ChristmasEve(int year) =>
        new(year, 12, 24);

    public static Date BoxingDay(int year) =>
        new(year, 12, 26);

    public static IEnumerable<(Date date, string name)> Get(int year)
    {
        var christmasDay = ChristmasDay(year);
        var boxingDay = BoxingDay(year);

        yield return (christmasDay, "Christmas Day");
        yield return (boxingDay, "Boxing Day");

        // Only add substitute days when Christmas or Boxing Day fall on a weekend
        var christmasOnWeekend = !christmasDay.IsWeekday();
        var boxingDayOnWeekend = !boxingDay.IsWeekday();

        if (christmasOnWeekend || boxingDayOnWeekend)
        {
            // Find the next weekday after Boxing Day for the first substitute
            var nextWeekday = boxingDay.AddDays(1);
            while (!nextWeekday.IsWeekday())
            {
                nextWeekday = nextWeekday.AddDays(1);
            }
            yield return (nextWeekday, "Christmas (additional)");

            // If BOTH Christmas and Boxing Day are on weekend, we need a second substitute
            // (i.e., Christmas on Saturday and Boxing Day on Sunday)
            if (christmasOnWeekend && boxingDayOnWeekend)
            {
                var secondWeekday = nextWeekday.AddDays(1);
                yield return (secondWeekday, "Christmas (additional)");
            }
        }
    }
}