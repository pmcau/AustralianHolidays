namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to iCalendar (ICS) format.
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
    /// Exports national public holidays to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToIcs(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = NationalForYears(startYear, yearCount);

        return ToIcs(writer, null, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for a specific state to iCalendar (ICS) format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
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
    /// Exports public holidays for a specific state to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToIcs(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var forYears = ForYears(state, startYear, yearCount);

        return ToIcs(writer, state, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to iCalendar (ICS) format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
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
    /// Exports public holidays for multiple states to iCalendar (ICS) format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the ICS output to.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToIcs(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToIcsMultiState(writer, forYears, cancel);
    }

    static async Task ToIcs(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears, Cancel cancel)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR");
        await writer.WriteLineAsync("VERSION:2.0");

        foreach (var item in forYears)
        {
            await writer.WriteLineAsync("BEGIN:VEVENT");
            await writer.WriteLineAsync($"SUMMARY:{item.name}");
            await writer.WriteLineAsync($"UID:{item.date:yyyyMMdd}_{item.name}_{state}@AustralianHolidays");
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{item.date:yyyyMMdd}");
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{item.date.AddDays(1):yyyyMMdd}");
            await writer.WriteLineAsync("END:VEVENT");
        }

        await writer.WriteLineAsync("END:VCALENDAR", cancel);
    }

    static readonly int allStatesCount = Enum.GetValues<State>().Length;

    static async Task ToIcsMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears, Cancel cancel)
    {
        writer.NewLine = "\r\n";
        await writer.WriteLineAsync("BEGIN:VCALENDAR", cancel);
        await writer.WriteLineAsync("VERSION:2.0", cancel);
        await writer.WriteLineAsync("PRODID:-//Australian Holidays//EN", cancel);

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

            await writer.WriteLineAsync("BEGIN:VEVENT", cancel);
            await writer.WriteLineAsync($"SUMMARY:{summary}", cancel);
            await writer.WriteLineAsync($"UID:{uid}", cancel);
            await writer.WriteLineAsync($"DTSTART;VALUE=DATE:{date:yyyyMMdd}", cancel);
            await writer.WriteLineAsync($"DTEND;VALUE=DATE:{date.AddDays(1):yyyyMMdd}", cancel);
            await writer.WriteLineAsync("END:VEVENT", cancel);
        }

        await writer.WriteLineAsync("END:VCALENDAR", cancel);
    }
}
