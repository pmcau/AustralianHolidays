namespace AustralianHolidays;

public static partial class Holidays
{
    public static async Task<string> ExportToCsv(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToCsv(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToCsv(writer, forYears);
    }

    public static async Task<string> ExportToCsv(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToCsv(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToCsv(writer, forYears);
    }

    public static async Task ExportToCsv(string path, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, startYear, yearCount);
    }

    public static async Task ExportToCsv(string path, State state, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, state, startYear, yearCount);
    }

    static async Task ToCsv(TextWriter writer, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        await writer.WriteLineAsync("Date,Name");
        foreach (var (date, name) in forYears)
        {
            var escapedName = name.Contains(',') || name.Contains('"')
                ? $"\"{name.Replace("\"", "\"\"")}\""
                : name;
            await writer.WriteLineAsync($"{date.ToString("yyyy-MM-dd")},{escapedName}");
        }
    }
}
