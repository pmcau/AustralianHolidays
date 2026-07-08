namespace AustralianHolidays;

public static partial class Holidays
{

    /// <summary>
    /// Exports national public holidays to JSON format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the JSON-formatted holiday data.</returns>
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
    /// Exports national public holidays to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToJson(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToJson(writer, null, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for a specific state to JSON format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the JSON-formatted holiday data.</returns>
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
    /// Exports public holidays for a specific state to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToJson(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToJson(writer, state, forYears, cancel);
    }

    /// <summary>
    /// Exports national public holidays to a JSON file.
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
    /// Exports public holidays for a specific state to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToJson(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to JSON format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the JSON-formatted holiday data with state information per holiday.</returns>
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
    /// Exports public holidays for multiple states to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToJson(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToJsonMultiState(writer, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToJson(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, states, startYear, yearCount, cancel);
    }

    static Task ToJson(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears, Cancel cancel)
    {
        var holidays = forYears
            .Select(_ => new HolidayJson(_.date.ToString("yyyy-MM-dd"), _.name));
        var result = new HolidayExportJson(state?.ToString() ?? "National", holidays);
        var json = JsonSerializer.Serialize(result, HolidayJsonContext.Default.HolidayExportJson);
        return writer.WriteAsync(json, cancel);
    }

    static Task ToJsonMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears, Cancel cancel)
    {
        var holidays = forYears
            .Select(_ => new MultiStateHolidayJson(_.date.ToString("yyyy-MM-dd"), _.state.ToString(), _.name));
        var json = JsonSerializer.Serialize(holidays, HolidayJsonContext.Default.IEnumerableMultiStateHolidayJson);
        return writer.WriteAsync(json, cancel);
    }
}
