namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to CSV format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the CSV-formatted holiday data with Date and Name columns.</returns>
    public static async Task<string> ExportToCsv(int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports national public holidays to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToCsv(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToCsv(writer, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for a specific state to CSV format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the CSV-formatted holiday data with Date and Name columns.</returns>
    public static async Task<string> ExportToCsv(State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, state, startYear, yearCount, cancel);
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
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToCsv(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToCsv(writer, forYears, cancel);
    }

    /// <summary>
    /// Exports national public holidays to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToCsv(string path, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports public holidays for a specific state to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToCsv(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to CSV format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the CSV-formatted holiday data with Date, State, and Name columns.</returns>
    public static async Task<string> ExportToCsv(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToCsv(writer, states, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports public holidays for multiple states to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToCsv(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToCsvMultiState(writer, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToCsv(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, states, startYear, yearCount, cancel);
    }

    static async Task ToCsv(TextWriter writer, IOrderedEnumerable<(Date date, string name)> forYears, Cancel cancel)
    {
        await writer.WriteLineAsync("Date,Name", cancel);
        foreach (var (date, name) in forYears)
        {
            var escapedName = name.Contains(',') || name.Contains('"')
                ? $"\"{name.Replace("\"", "\"\"")}\""
                : name;
            await writer.WriteLineAsync($"{date:yyyy-MM-dd},{escapedName}", cancel);
        }
    }

    static async Task ToCsvMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears, Cancel cancel)
    {
        await writer.WriteLineAsync("Date,State,Name", cancel);
        foreach (var (date, state, name) in forYears)
        {
            var escapedName = name.Contains(',') || name.Contains('"')
                ? $"\"{name.Replace("\"", "\"\"")}\""
                : name;
            await writer.WriteLineAsync($"{date:yyyy-MM-dd},{state},{escapedName}", cancel);
        }
    }
}
