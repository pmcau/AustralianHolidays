namespace AustralianHolidays;

public static partial class Holidays
{
    public static string ExportToMarkdown(int startYear, int yearCount = 5)
    {
        var years = BuildYears(startYear);

        var forYears = ForYearsFederal(startYear, yearCount);

        return ToMarkdown(years, forYears);
    }

    public static string ExportToMarkdown(State state, int? startYear = null, int yearCount = 5)
    {
        var years = BuildYears(startYear);

        var forYears = ForYears(state, startYear, yearCount);

        return ToMarkdown(years, forYears);
    }

    internal static string ToMarkdown(List<int> years, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"|                                   | {string.Join("         | ", years)}         |");
        builder.Append('|');
        builder.Append("-----------------------------------|");
        for (var index = 1; index < years.Count + 1; index++)
        {
            builder.Append("--------------|");
        }

        builder.AppendLine();

        var items = forYears.GroupBy(_ => _.name)
            .OrderBy(_ => _.First().date.Month)
            .ThenBy(_ => _.First().date.Day);

        foreach (var item in items)
        {
            builder.Append("| " + item.Key.Replace(" (", "<br>(").PadRight(33) + " | ");
            foreach (var year in years)
            {
                var dates = item.Select(_ => _.date)
                    .Where(_ => _.Year == year)
                    .ToList();
                if (dates.Count == 0)
                {
                    builder.Append("            ");
                }
                else
                {
                    builder.Append(string.Join("<br>", dates.Select(_ => _.ToString("`ddd dd MMM`", CultureInfo.InvariantCulture))));
                }

                builder.Append(" | ");
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}