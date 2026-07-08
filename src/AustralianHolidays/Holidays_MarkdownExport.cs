namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to Markdown table format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the Markdown-formatted table of holidays.</returns>
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
    /// Exports national public holidays to Markdown table format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the Markdown output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToMarkdown(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var years = BuildYears(startYear);

        var forYears = NationalForYears(startYear, yearCount);

        return ToMarkdown(writer, years, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for a specific state to Markdown table format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the Markdown-formatted table of holidays.</returns>
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
    /// Exports public holidays for a specific state to Markdown table format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the Markdown output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToMarkdown(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var years = BuildYears(startYear);

        var forYears = ForYears(state, startYear, yearCount);

        return ToMarkdown(writer, years, forYears, cancel);
    }

    /// <summary>
    /// Exports public holidays for multiple states to Markdown table format.
    /// </summary>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the Markdown-formatted table of holidays with state information.</returns>
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
    /// Exports public holidays for multiple states to Markdown table format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the Markdown output to.</param>
    /// <param name="states">The Australian states to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToMarkdown(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var forYears = ForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 || stateSet.Contains(_.state));
        return ToMarkdownMultiState(writer, forYears, cancel);
    }

    static async Task ToMarkdown(TextWriter writer, List<int> years, IOrderedEnumerable<(Date date, string name)> forYears, Cancel cancel)
    {
        await writer.WriteLineAsync($"|                                   | {string.Join("         | ", years)}         |", cancel);
        await writer.WriteAsync("|-----------------------------------|", cancel);
        for (var index = 1; index < years.Count + 1; index++)
        {
            await writer.WriteAsync("--------------|", cancel);
        }

        await writer.WriteLineAsync(cancel);

        var items = forYears.GroupBy(_ => _.name)
            .OrderBy(_ => _.First().date.Month)
            .ThenBy(_ => _.First().date.Day);

        foreach (var item in items)
        {
            await writer.WriteAsync("| " + item.Key.Replace(" (", "<br>(").PadRight(33) + " | ", cancel);
            foreach (var year in years)
            {
                var dates = item.Select(_ => _.date)
                    .Where(_ => _.Year == year)
                    .ToList();
                if (dates.Count == 0)
                {
                    await writer.WriteAsync("            ", cancel);
                }
                else
                {
                    await writer.WriteAsync(string.Join("<br>", dates.Select(_ => _.ToString("`ddd dd MMM`", CultureInfo.InvariantCulture))), cancel);
                }

                await writer.WriteAsync(" | ", cancel);
            }

            await writer.WriteLineAsync(cancel);
        }
    }

    static async Task ToMarkdownMultiState(TextWriter writer, IEnumerable<(Date date, State state, string name)> forYears, Cancel cancel)
    {
        await writer.WriteLineAsync("# Australian Public Holidays", cancel);
        await writer.WriteLineAsync(cancel);
        await writer.WriteLineAsync("| Date | State | Holiday |", cancel);
        await writer.WriteLineAsync("|------|-------|---------|", cancel);

        foreach (var (date, state, name) in forYears)
        {
            await writer.WriteLineAsync($"| {date:yyyy-MM-dd} | {state} | {name} |", cancel);
        }
    }
}