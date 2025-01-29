namespace AustralianHolidays;

public static class ChristmasCalculator
{
    public static bool IsChristmasDay(this Date date) =>
        date == ChristmasDay(date.Year);

    public static bool IsChristmasEve(this Date date) =>
        date == ChristmasEve(date.Year);

    public static Date ChristmasDay(int year) =>
        new(year, 12, 25);

    public static Date ChristmasEve(int year) =>
        new(year, 12, 24);

    public static Date BoxingDay(int year) =>
        new(year, 12, 26);

    public static bool IsBoxingDay(this Date date) =>
        date == BoxingDay(date.Year);

    public static bool TryGet(Date date, [NotNullWhen(true)] out string? name)
    {
        var christmasDay = ChristmasDay(date.Year);
        if (date == christmasDay)
        {
            name = "Christmas Day";
            return true;
        }

        var boxingDay = BoxingDay(date.Year);
        if (date == boxingDay)
        {
            name = "Boxing Day";
            return true;
        }

        var boxingDayPlus1 = boxingDay.AddDays(1);
        if (date == boxingDayPlus1 && boxingDayPlus1.IsWeekday())
        {
            name = "Christmas Holiday";
            return true;
        }

        var boxingDayPlus2 = boxingDay.AddDays(2);
        if (date == boxingDayPlus2 && boxingDayPlus2.IsWeekday())
        {
            name = "Christmas Holiday";
            return true;
        }

        name = null;
        return false;
    }
}