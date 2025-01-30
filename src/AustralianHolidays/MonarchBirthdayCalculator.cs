static class MonarchBirthdayCalculator
{
    public static bool IsMonarchBirthday(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsSecondMonday(Month.June))
        {
            if (date.Year <= 2022)
            {
                name = "Queen's Birthday";
                return true;
            }

            if (date.Year > 2022)
            {
                name = "King's Birthday";
                return true;
            }
        }

        name = null;
        return false;
    }

    public static bool IsMonarchBirthdayQld(this Date date, [NotNullWhen(true)] out string? name)
    {
        if (date.IsFirstMonday(Month.October))
        {
            if (date.Year <= 2022)
            {
                name = "Queen's Birthday";
                return true;
            }

            if (date.Year > 2022)
            {
                name = "King's Birthday";
                return true;
            }
        }

        name = null;
        return false;
    }

    public static bool IsMonarchBirthdayWa(this Date date, [NotNullWhen(true)] out string? name)
    {
        name = null;
        if (date.Month != (int)Month.September)
        {
            return false;
        }

        if (date.DayOfWeek != DayOfWeek.Monday)
        {
            return false;
        }

        if (date.Day < 23)
        {
            return false;
        }

        var birthday = GetBirthday(date.Year);
        if (birthday == date)
        {
            if (date.Year > 2022)
            {
                name = "King's Birthday";
            }
            else
            {
                name = "Queen's Birthday";
            }

            return true;
        }

        return false;

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