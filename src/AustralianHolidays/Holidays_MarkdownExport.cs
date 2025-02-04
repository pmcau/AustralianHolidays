namespace AustralianHolidays;

public static partial class Holidays
{
    public static async Task<string> ExportToMarkdown(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToMarkdown(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToMarkdown(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var years = BuildYears(startYear);

        var forYears = NationalForYears(startYear, yearCount);

        return ToMarkdown(writer, years, forYears);
    }

    public static async Task<string> ExportToMarkdown(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToMarkdown(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToMarkdown(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var years = BuildYears(startYear);

        var forYears = ForYears(state, startYear, yearCount);

        return ToMarkdown(writer, years, forYears);
    }

    internal static async Task ToMarkdown(TextWriter writer, List<int> years, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        await writer.WriteLineAsync($"|                                   | {string.Join("         | ", years)}         |");
        await writer.WriteAsync('|');
        await writer.WriteAsync("-----------------------------------|");
        for (var index = 1; index < years.Count + 1; index++)
        {
            await writer.WriteAsync("--------------|");
        }

        await writer.WriteLineAsync();

        var items = forYears.GroupBy(_ => _.name)
            .OrderBy(_ => _.First().date.Month)
            .ThenBy(_ => _.First().date.Day);

        foreach (var item in items)
        {
            await writer.WriteAsync("| " + item.Key.Replace(" (", "<br>(").PadRight(33) + " | ");
            foreach (var year in years)
            {
                var dates = item.Select(_ => _.date)
                    .Where(_ => _.Year == year)
                    .ToList();
                if (dates.Count == 0)
                {
                    await writer.WriteAsync("            ");
                }
                else
                {
                    await writer.WriteAsync(string.Join("<br>", dates.Select(_ => _.ToString("`ddd dd MMM`", CultureInfo.InvariantCulture))));
                }

                await writer.WriteAsync(" | ");
            }

            await writer.WriteLineAsync();
        }
    }
}