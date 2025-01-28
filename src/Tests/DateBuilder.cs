public class DateBuilder
{
    public static  IEnumerable<Date> Range()
    {
        for (var year = 2024; year <= 2035; year++)
        {
            var start = new Date(year, 1, 1);
            var end = new Date(year, 12, 31);

            for (var date = start; date <= end; date = date.AddDays(1))
            {

                yield return date;
            }
        }
    }

    public static List<(Date date, string name)> Build()
    {
        var items = new List<(Date date, string name)>();

        for (var year = 2024; year <= 2044; year++)
        {
            var australiaDay = new Date(year, 1, 26);
            items.Add((australiaDay.AddDays(-1), "Australia Day - 1"));
            items.Add((australiaDay, "Australia Day"));
            items.Add((australiaDay.AddDays(1), "Australia Day + 1"));
            items.Add((australiaDay.AddDays(2), "Australia Day + 2"));

            var anzacDay = new Date(year, 4, 25);
            items.Add((anzacDay.AddDays(-1), "Anzac Day - 1"));
            items.Add((anzacDay, "Anzac Day"));
            items.Add((anzacDay.AddDays(1), "Anzac Day + 1"));
            items.Add((anzacDay.AddDays(2), "Anzac Day + 2"));

            var xmas = new Date(year, 12, 25);
            items.Add((xmas.AddDays(-1), "Xmas - 1"));
            items.Add((xmas, "Xmas"));

            var boxingDay = new Date(year, 12, 26);
            items.Add((boxingDay, "Boxing Day"));
            items.Add((boxingDay.AddDays(1), "Boxing Day + 1"));
            items.Add((boxingDay.AddDays(2), "Boxing Day + 2"));

            var newYears = new Date(year, 1, 1);
            items.Add((newYears.AddDays(-1), "New Years - 1"));
            items.Add((newYears, "New Years"));
            items.Add((newYears.AddDays(1), "New Years + 1"));
            items.Add((newYears.AddDays(2), "New Years + 2"));
        }

        foreach (var easterFriday in EasterFridays)
        {
            items.Add((easterFriday.AddDays(-1), "Before Easter"));
            items.Add((easterFriday, "Good Friday"));
            items.Add((easterFriday.AddDays(1), "Easter Saturday"));
            items.Add((easterFriday.AddDays(2), "Easter Sunday"));
            items.Add((easterFriday.AddDays(3), "Easter Monday"));
            items.Add((easterFriday.AddDays(4), "After Easter"));
        }

        return items.OrderBy(_ => _.date).ToList();
    }

    public static List<Date> EasterFridays =
    [
        new(2025, 4, 18),
        new(2026, 4, 3),
        new(2027, 3, 26),
        new(2028, 4, 14),
        new(2029, 3, 30),
        new(2030, 4, 19),
        new(2031, 4, 11),
        new(2032, 3, 26),
        new(2033, 4, 15),
        new(2034, 4, 7),
        new(2035, 3, 23),
        new(2036, 4, 11),
        new(2037, 4, 3),
        new(2038, 4, 23),
        new(2039, 4, 8),
        new(2040, 3, 30),
        new(2041, 4, 19),
        new(2042, 4, 4),
        new(2043, 3, 27),
        new(2044, 4, 15),
        new(2045, 4, 7),
        new(2046, 3, 23),
        new(2047, 4, 12),
        new(2048, 4, 3),
        new(2049, 4, 23),
        new(2050, 4, 8),
    ];
}