using System.Text.Json;

namespace AustralianHolidays;

public static partial class Holidays
{
    static readonly JsonSerializerOptions jsonOptions = new() {WriteIndented = true};

    public static async Task<string> ExportToJson(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToJson(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToJson(writer, null, forYears);
    }

    public static async Task<string> ExportToJson(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToJson(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToJson(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToJson(writer, state, forYears);
    }

    static async Task ToJson(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        var holidays = forYears.Select(h => new {date = h.date.ToString("yyyy-MM-dd"), name = h.name});
        var result = new {state = state?.ToString() ?? "National", holidays};
        var json = JsonSerializer.Serialize(result, jsonOptions);
        await writer.WriteAsync(json);
    }
}
