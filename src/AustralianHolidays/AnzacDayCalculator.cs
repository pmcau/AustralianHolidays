namespace AustralianHolidays;

public static class AnzacDayCalculator
{
    public static bool IsAnzacDay(this Date date) =>
        date is { Month: 4, Day: 25 };

    public static Date GetAnzacDay(int year) => new(year, April, 25);
}