namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <summary>
    /// Exports school holiday periods for all states to iCalendar (ICS) format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the ICS-formatted calendar data.</returns>
    public static async Task<string> ExportToIcs(int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for all states to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToIcs(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(startYear, yearCount);
        return ToIcsMultiState(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to iCalendar (ICS) format.
    /// </summary>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the ICS-formatted calendar data.</returns>
    public static async Task<string> ExportToIcs(State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, state, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToIcs(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(state, startYear, yearCount);
        return ToIcs(writer, state, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to iCalendar (ICS) format.
    /// </summary>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the ICS-formatted calendar data with state information.</returns>
    public static async Task<string> ExportToIcs(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToIcs(writer, states, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToIcs(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var holidays = HolidaysForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToIcsMultiState(writer, holidays, cancel);
    }

    static async Task ToIcs(TextWriter writer, State state, IEnumerable<(Date start, Date end, string name)> holidays, Cancel cancel)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR", cancel);
        await writer.WriteLineAsync("VERSION:2.0", cancel);
        await writer.WriteLineAsync("PRODID:-//Australian Holidays//EN", cancel);

        foreach (var (start, end, name) in holidays)
        {
            await writer.WriteLineAsync("BEGIN:VEVENT", cancel);
            await writer.WriteLineAsync($"SUMMARY:{name} school holidays", cancel);
            await writer.WriteLineAsync($"UID:{start:yyyyMMdd}_{name}_{state}@AustralianHolidays", cancel);
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{start:yyyyMMdd}", cancel);
            // DTEND is exclusive for all-day events, so add a day to include the final day of the break.
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{end.AddDays(1):yyyyMMdd}", cancel);
            await writer.WriteLineAsync("END:VEVENT", cancel);
        }

        await writer.WriteLineAsync("END:VCALENDAR", cancel);
    }

    static async Task ToIcsMultiState(TextWriter writer, IEnumerable<(Date start, Date end, State state, string name)> holidays, Cancel cancel)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR", cancel);
        await writer.WriteLineAsync("VERSION:2.0", cancel);
        await writer.WriteLineAsync("PRODID:-//Australian Holidays//EN", cancel);

        // School break dates differ by state, so unlike public holidays they are not merged across states.
        foreach (var (start, end, state, name) in holidays)
        {
            await writer.WriteLineAsync("BEGIN:VEVENT", cancel);
            await writer.WriteLineAsync($"SUMMARY:{state} {name} school holidays", cancel);
            await writer.WriteLineAsync($"UID:{start:yyyyMMdd}_{name}_{state}@AustralianHolidays", cancel);
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{start:yyyyMMdd}", cancel);
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{end.AddDays(1):yyyyMMdd}", cancel);
            await writer.WriteLineAsync("END:VEVENT", cancel);
        }

        await writer.WriteLineAsync("END:VCALENDAR", cancel);
    }
}
