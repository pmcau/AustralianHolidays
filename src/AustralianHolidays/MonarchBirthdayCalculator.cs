namespace AustralianHolidays;

public static class MonarchBirthdayCalculator
{
    public static bool IsMonarchBirthday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsQueensBirthday())
        {
            name = "Queens Birthday";
            return true;
        }

        if (date.IsKingsBirthday())
        {
            name = "Kings Birthday";
            return true;
        }

        name = null;
        return false;
    }

    public static bool IsKingsBirthday(this Date date) =>
        date.Year > 2022 &&
        date.IsSecondMonday(Month.June);

    public static bool IsQueensBirthday(this Date date) =>
        date.Year <= 2022 &&
        date.IsSecondMonday(Month.June);

}