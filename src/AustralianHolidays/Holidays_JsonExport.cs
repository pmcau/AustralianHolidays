using System.Text.Json;

namespace AustralianHolidays;

public static partial class Holidays
{

    /// <summary>
    /// Exports national public holidays to JSON format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the JSON-formatted holiday data.</returns>
    public static async Task<string> ExportToJson(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports national public holidays to JSON format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the JSON output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToJson(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToJson(writer, null, forYears);
    }

    /// <summary>
    /// Exports public holidays for a specific state to JSON format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the JSON-formatted holiday data.</returns>
    public static async Task<string> ExportToJson(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, state, startYear, yearCount);
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
    public static Task ExportToJson(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToJson(writer, state, forYears);
    }

    /// <summary>
    /// Exports national public holidays to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToJson(string path, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, startYear, yearCount);
    }

    /// <summary>
    /// Exports public holidays for a specific state to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToJson(string path, State state, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, state, startYear, yearCount);
    }

    /// <summary>
    /// Exports public holidays for multiple states to JSON format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the JSON-formatted holiday data with state information per holiday.</returns>
    public static async Task<string> ExportToJson(IEnumerable<State> states, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, states, startYear, yearCount);
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
    public static Task ExportToJson(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(h => stateSet.Count == 0 || stateSet.Contains(h.state));
        return ToJsonMultiState(writer, forYears);
    }

    /// <summary>
    /// Exports public holidays for multiple states to a JSON file.
    /// </summary>
    /// <param name="path">The file path where the JSON data will be written.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToJson(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToJson(writer, states, startYear, yearCount);
    }

    static Task ToJson(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        var holidays = forYears
            .Select(h => new HolidayJson(h.date.ToString("yyyy-MM-dd"), h.name));
        var result = new HolidayExportJson(state?.ToString() ?? "National", holidays);
        var json = JsonSerializer.Serialize(result, HolidayJsonContext.Default.HolidayExportJson);
        return writer.WriteAsync(json);
    }

    static Task ToJsonMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears)
    {
        var holidays = forYears
            .Select(h => new MultiStateHolidayJson(h.date.ToString("yyyy-MM-dd"), h.state.ToString(), h.name));
        var json = JsonSerializer.Serialize(holidays, HolidayJsonContext.Default.IEnumerableMultiStateHolidayJson);
        return writer.WriteAsync(json);
    }
}
