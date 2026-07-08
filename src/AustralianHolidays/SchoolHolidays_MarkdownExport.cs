namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    static readonly string[] seasonOrder = ["Summer", "Autumn", "Winter", "Spring"];

    /// <summary>
    /// Exports school holiday periods for all states to Markdown table format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the Markdown-formatted table.</returns>
    public static async Task<string> ExportToMarkdown(int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToMarkdown(writer, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for all states to Markdown table format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the Markdown output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToMarkdown(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(startYear, yearCount);
        return ToMarkdownMultiState(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to Markdown table format.
    /// </summary>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the Markdown-formatted table with a row per season and a column per year.</returns>
    public static async Task<string> ExportToMarkdown(State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToMarkdown(writer, state, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to Markdown table format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the Markdown output to.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToMarkdown(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(state, startYear, yearCount);
        return ToMarkdown(writer, holidays, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to Markdown table format.
    /// </summary>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the Markdown-formatted table with state information.</returns>
    public static async Task<string> ExportToMarkdown(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToMarkdown(writer, states, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to Markdown table format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the Markdown output to.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToMarkdown(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var holidays = HolidaysForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToMarkdownMultiState(writer, holidays, cancel);
    }

    static async Task ToMarkdown(TextWriter writer, IEnumerable<(Date start, Date end, string name)> source, Cancel cancel)
    {
        var list = source.ToList();
        // A period is owned by the year it ends in (Summer starts in the previous December but ends in
        // the new year), so the year columns are derived from the end dates.
        var years = list
            .Select(_ => _.end.Year)
            .Distinct()
            .Order()
            .ToList();

        var header = new StringBuilder("| Season |");
        var separator = new StringBuilder("| --- |");
        foreach (var year in years)
        {
            header.Append($" {year} |");
            separator.Append(" --- |");
        }

        await writer.WriteLineAsync(header.ToString(), cancel);
        await writer.WriteLineAsync(separator.ToString(), cancel);

        var byName = list
            .GroupBy(_ => _.name)
            .ToDictionary(_ => _.Key, _ => _.ToList());

        foreach (var season in seasonOrder)
        {
            if (!byName.TryGetValue(season, out var periods))
            {
                continue;
            }

            var row = new StringBuilder($"| {season} |");
            foreach (var year in years)
            {
                var matches = periods
                    .Where(_ => _.end.Year == year)
                    .ToList();
                if (matches.Count == 0)
                {
                    row.Append("  |");
                }
                else
                {
                    var (start, end, _) = matches[0];
                    row.Append($" {FormatRange(start, end)} |");
                }
            }

            await writer.WriteLineAsync(row.ToString(), cancel);
        }
    }

    static async Task ToMarkdownMultiState(TextWriter writer, IEnumerable<(Date start, Date end, State state, string name)> holidays, Cancel cancel)
    {
        await writer.WriteLineAsync("# Australian School Holidays", cancel);
        await writer.WriteLineAsync(cancel);
        await writer.WriteLineAsync("| Start | End | State | Season |", cancel);
        await writer.WriteLineAsync("| --- | --- | --- | --- |", cancel);

        foreach (var (start, end, state, name) in holidays)
        {
            await writer.WriteLineAsync($"| {start.ToString("yyyy-MM-dd")} | {end.ToString("yyyy-MM-dd")} | {state} | {name} |", cancel);
        }
    }

    static string FormatRange(Date start, Date end) =>
        $"`{start.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} - {end.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}`";
}
