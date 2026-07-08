namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <summary>
    /// Exports school holiday periods for all states to CSV format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the CSV-formatted data with Start, End, State, and Name columns.</returns>
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
    /// Exports school holiday periods for all states to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToCsv(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(startYear, yearCount);
        return ToCsvMultiState(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to CSV format.
    /// </summary>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the CSV-formatted data with Start, End, and Name columns.</returns>
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
    /// Exports school holiday periods for a specific state to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToCsv(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(state, startYear, yearCount);
        return ToCsv(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for all states to a CSV file.
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
    /// Exports school holiday periods for a specific state to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToCsv(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to CSV format.
    /// </summary>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the CSV-formatted data with Start, End, State, and Name columns.</returns>
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
    /// Exports school holiday periods for multiple states to CSV format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the CSV output to.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToCsv(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var holidays = HolidaysForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToCsvMultiState(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to a CSV file.
    /// </summary>
    /// <param name="path">The file path where the CSV data will be written.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToCsv(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToCsv(writer, states, startYear, yearCount, cancel);
    }

    static async Task ToCsv(TextWriter writer, IEnumerable<(Date start, Date end, string name)> holidays, Cancel cancel)
    {
        await writer.WriteLineAsync("Start,End,Name", cancel);
        foreach (var (start, end, name) in holidays)
        {
            await writer.WriteLineAsync($"{start.ToString("yyyy-MM-dd")},{end.ToString("yyyy-MM-dd")},{Escape(name)}", cancel);
        }
    }

    static async Task ToCsvMultiState(TextWriter writer, IEnumerable<(Date start, Date end, State state, string name)> holidays, Cancel cancel)
    {
        await writer.WriteLineAsync("Start,End,State,Name", cancel);
        foreach (var (start, end, state, name) in holidays)
        {
            await writer.WriteLineAsync($"{start.ToString("yyyy-MM-dd")},{end.ToString("yyyy-MM-dd")},{state},{Escape(name)}", cancel);
        }
    }

    static string Escape(string name) =>
        name.Contains(',') || name.Contains('"')
            ? $"\"{name.Replace("\"", "\"\"")}\""
            : name;
}
