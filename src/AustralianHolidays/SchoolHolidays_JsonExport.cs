namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <summary>
    /// Exports school holiday periods for all states to JSON format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the JSON-formatted data.</returns>
    public static async Task<string> ExportToJson(int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for all states to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToJson(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(startYear, yearCount);
        return ToJsonMultiState(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to JSON format.
    /// </summary>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the JSON-formatted data.</returns>
    public static async Task<string> ExportToJson(State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, state, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToJson(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(state, startYear, yearCount);
        return ToJson(writer, state, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for all states to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToJson(string path, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToJson(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to JSON format.
    /// </summary>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the JSON-formatted data with state information per period.</returns>
    public static async Task<string> ExportToJson(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, states, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToJson(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var holidays = HolidaysForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToJsonMultiState(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToJson(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, states, startYear, yearCount, cancel);
    }

    static Task ToJson(TextWriter writer, State state, IEnumerable<(Date start, Date end, string name)> holidays, Cancel cancel)
    {
        var mapped = holidays
            .Select(_ => new SchoolHolidayJson(_.start.ToString("yyyy-MM-dd"), _.end.ToString("yyyy-MM-dd"), _.name));
        var result = new SchoolHolidayExportJson(state.ToString(), mapped);
        var json = JsonSerializer.Serialize(result, SchoolHolidayJsonContext.Default.SchoolHolidayExportJson);
        return writer.WriteAsync(json, cancel);
    }

    static Task ToJsonMultiState(TextWriter writer, IEnumerable<(Date start, Date end, State state, string name)> holidays, Cancel cancel)
    {
        var mapped = holidays
            .Select(_ => new MultiStateSchoolHolidayJson(_.start.ToString("yyyy-MM-dd"), _.end.ToString("yyyy-MM-dd"), _.state.ToString(), _.name));
        var json = JsonSerializer.Serialize(mapped, SchoolHolidayJsonContext.Default.IEnumerableMultiStateSchoolHolidayJson);
        return writer.WriteAsync(json, cancel);
    }
}
