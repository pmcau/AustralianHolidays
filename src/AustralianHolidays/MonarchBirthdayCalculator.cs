static class MonarchBirthdayCalculator
{
    public static (Date, string) GetMonarchBirthday(int year)
    {
        var date = Extensions.GetSecondMonday(Month.June, year);
        if (year <= 2022)
        {
            return (date, "Queen's Birthday");
        }

        return (date, "King's Birthday");
    }

    public static (Date, string) GetMonarchBirthdayQld(int year)
    {
        var date = Extensions.GetFirstMonday(Month.October, year);
        if (year <= 2022)
        {
            return (date, "Queen's Birthday");
        }

        return (date, "King's Birthday");
    }

    public static (Date, string) GetMonarchBirthdayWa(int year)
    {
        var birthday = GetBirthday(year);
        if (year > 2022)
        {
            return (birthday, "King's Birthday");
        }

        return (birthday, "Queen's Birthday");

        static Date GetBirthday(int year)
        {
            var date = new Date(year, 9, 23);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}