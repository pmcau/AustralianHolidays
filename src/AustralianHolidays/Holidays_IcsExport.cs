namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to iCalendar (ICS) format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the ICS-formatted calendar data.</returns>
    public static async Task<string> ExportToIcs(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports national public holidays to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToIcs(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);

        return ToIcs(writer, null, forYears);
    }

    /// <summary>
    /// Exports public holidays for a specific state to iCalendar (ICS) format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the ICS-formatted calendar data.</returns>
    public static async Task<string> ExportToIcs(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports public holidays for a specific state to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToIcs(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);

        return ToIcs(writer, state, forYears);
    }

    /// <summary>
    /// Exports public holidays for multiple states to iCalendar (ICS) format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the ICS-formatted calendar data with state information.</returns>
    public static async Task<string> ExportToIcs(IEnumerable<State> states, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, states, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports public holidays for multiple states to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToIcs(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(h => stateSet.Count == 0 || stateSet.Contains(h.state));
        return ToIcsMultiState(writer, forYears);
    }

    static async Task ToIcs(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR");
        await writer.WriteLineAsync("VERSION:2.0");

        foreach (var item in forYears)
        {
            await writer.WriteLineAsync("BEGIN:VEVENT");
            await writer.WriteLineAsync($"SUMMARY:{item.name}");
            await writer.WriteLineAsync($"UID:{item.name}_{state}@AustralianHolidays");
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{item.date:yyyyMMdd}");
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{item.date.AddDays(1):yyyyMMdd}");
            await writer.WriteLineAsync("END:VEVENT");
        }

        await writer.WriteLineAsync("END:VCALENDAR");
    }

    static readonly int AllStatesCount = Enum.GetValues<State>().Length;

    static async Task ToIcsMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR");
        await writer.WriteLineAsync("VERSION:2.0");
        await writer.WriteLineAsync("PRODID:-//Australian Holidays//EN");

        // Group by date and name to detect holidays that apply to all states
        var grouped = forYears
            .GroupBy(h => (h.date, h.name))
            .Select(g => (g.Key.date, g.Key.name, states: g.Select(h => h.state).ToList()));

        foreach (var (date, name, states) in grouped)
        {
            // If all states have this holiday with same date and name, merge into single entry
            if (states.Count == AllStatesCount)
            {
                await writer.WriteLineAsync("BEGIN:VEVENT");
                await writer.WriteLineAsync($"SUMMARY:{name}");
                await writer.WriteLineAsync($"UID:{date:yyyyMMdd}_{name}@AustralianHolidays");
                await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{date:yyyyMMdd}");
                await writer.WriteLineAsync($"DTEND;VALUE=DATE:{date.AddDays(1):yyyyMMdd}");
                await writer.WriteLineAsync("END:VEVENT");
            }
            else
            {
                // Output separate entries for each state
                foreach (var state in states)
                {
                    await writer.WriteLineAsync("BEGIN:VEVENT");
                    await writer.WriteLineAsync($"SUMMARY:{name} ({state})");
                    await writer.WriteLineAsync($"UID:{date:yyyyMMdd}_{name}_{state}@AustralianHolidays");
                    await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{date:yyyyMMdd}");
                    await writer.WriteLineAsync($"DTEND;VALUE=DATE:{date.AddDays(1):yyyyMMdd}");
                    await writer.WriteLineAsync("END:VEVENT");
                }
            }
        }

        await writer.WriteLineAsync("END:VCALENDAR");
    }
}