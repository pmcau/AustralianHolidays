namespace AustralianHolidays;

static class MonarchBirthdayCalculator
{
    public static bool IsMonarchBirthday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsQueensBirthday())
        {
            name = "Queen's Birthday";
            return true;
        }

        if (date.IsKingsBirthday())
        {
            name = "King's Birthday";
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

    public static bool IsMonarchBirthdayQld(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsQueensBirthdayQld())
        {
            name = "Queen's Birthday";
            return true;
        }

        if (date.IsKingsBirthdayQld())
        {
            name = "King's Birthday";
            return true;
        }

        name = null;
        return false;
    }

    public static bool IsKingsBirthdayQld(this Date date) =>
        date.Year > 2022 &&
        date.IsFirstMonday(Month.October);

    public static bool IsQueensBirthdayQld(this Date date) =>
        date.Year <= 2022 &&
        date.IsFirstMonday(Month.October);

    public static bool IsMonarchBirthdayWa(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsQueensBirthdayWa())
        {
            name = "Queen's Birthday";
            return true;
        }

        if (date.IsKingsBirthdayWa())
        {
            name = "King's Birthday";
            return true;
        }

        name = null;
        return false;
    }

    public static bool IsKingsBirthdayWa(this Date date) =>
        date.Year > 2022 &&
        date.IsLastMondayInMonth(Month.September);

    public static bool IsQueensBirthdayWa(this Date date) =>
        date.Year <= 2022 &&
        date.IsLastMondayInMonth(Month.September);

}