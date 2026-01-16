namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to CSV format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the CSV-formatted holiday data with Date and Name columns.</returns>
    public static async Task<string> ExportToCsv(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports national public holidays to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToCsv(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToCsv(writer, forYears);
    }

    /// <summary>
    /// Exports public holidays for a specific state to CSV format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the CSV-formatted holiday data with Date and Name columns.</returns>
    public static async Task<string> ExportToCsv(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports public holidays for a specific state to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToCsv(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToCsv(writer, forYears);
    }

    /// <summary>
    /// Exports national public holidays to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToCsv(string path, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, startYear, yearCount);
    }

    /// <summary>
    /// Exports public holidays for a specific state to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
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
