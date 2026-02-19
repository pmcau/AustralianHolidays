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

    static readonly int allStatesCount = Enum.GetValues<State>().Length;

    static async Task ToIcsMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR");
        await writer.WriteLineAsync("VERSION:2.0");
        await writer.WriteLineAsync("PRODID:-//Australian Holidays//EN");

        // Group by date and name to merge holidays across states
        var grouped = forYears
            .GroupBy(_ => (_.date, _.name))
            .Select(_ => (
                _.Key.date,
                _.Key.name,
                states: _.Select(_ => _.state).ToList()));

        foreach (var (date, name, states) in grouped)
        {
            string summary;
            string uid;

            if (states.Count == allStatesCount)
            {
                // All states have this holiday - no suffix needed
                summary = name;
                uid = $"{date:yyyyMMdd}_{name}@AustralianHolidays";
            }
            else
            {
                // Merge states into single entry with combined suffix
                var stateList = string.Join(", ", states);
                summary = $"{name} ({stateList})";
                uid = $"{date:yyyyMMdd}_{name}_{stateList}@AustralianHolidays";
            }

            await writer.WriteLineAsync("BEGIN:VEVENT");
            await writer.WriteLineAsync($"SUMMARY:{summary}");
            await writer.WriteLineAsync($"UID:{uid}");
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{date:yyyyMMdd}");
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{date.AddDays(1):yyyyMMdd}");
            await writer.WriteLineAsync("END:VEVENT");
        }

        await writer.WriteLineAsync("END:VCALENDAR");
    }
}
