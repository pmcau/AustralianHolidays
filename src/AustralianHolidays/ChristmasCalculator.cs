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
        yield return (ChristmasDay(year), "Christmas Day");
        var boxingDay = BoxingDay(year);
        yield return (boxingDay, "Boxing Day");

        var boxingDayPlus1 = boxingDay.AddDays(1);
        if (boxingDayPlus1.IsWeekday())
        {
            yield return (boxingDayPlus1, "Christmas (additional)");
        }

        var boxingDayPlus2 = boxingDay.AddDays(2);
        if (boxingDayPlus2.IsWeekday())
        {
            yield return (boxingDayPlus2, "Christmas (additional)");
        }
    }
}