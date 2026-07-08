namespace AustralianHolidays;

public static partial class SchoolHolidays
{
    /// <summary>
    /// Exports school holiday periods for all states to XML format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the XML-formatted data.</returns>
    public static async Task<string> ExportToXml(int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for all states to XML format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the XML output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToXml(TextWriter writer, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(startYear, yearCount);
        return ToXmlMultiState(writer, holidays);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to XML format.
    /// </summary>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the XML-formatted data.</returns>
    public static async Task<string> ExportToXml(State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, state, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to XML format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the XML output to.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToXml(TextWriter writer, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var holidays = HolidaysForYears(state, startYear, yearCount);
        return ToXml(writer, state, holidays);
    }

    /// <summary>
    /// Exports school holiday periods for all states to an XML file.
    /// </summary>
    /// <param name="path">The file path where the XML data will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToXml(string path, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToXml(writer, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for a specific state to an XML file.
    /// </summary>
    /// <param name="path">The file path where the XML data will be written.</param>
    /// <param name="state">The Australian state to export school holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToXml(string path, State state, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToXml(writer, state, startYear, yearCount, cancel);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to XML format.
    /// </summary>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A string containing the XML-formatted data with state information per period.</returns>
    public static async Task<string> ExportToXml(IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, states, startYear, yearCount, cancel);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to XML format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the XML output to.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static Task ExportToXml(TextWriter writer, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        var stateSet = states as IReadOnlySet<State> ?? states.ToHashSet();
        var holidays = HolidaysForYears(startYear, yearCount)
            .Where(_ => stateSet.Count == 0 ||
                        stateSet.Contains(_.state));
        return ToXmlMultiState(writer, holidays);
    }

    /// <summary>
    /// Exports school holiday periods for multiple states to an XML file.
    /// </summary>
    /// <param name="path">The file path where the XML data will be written.</param>
    /// <param name="states">The Australian states to export school holidays for. An empty collection exports all states.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <param name="cancel">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    public static async Task ExportToXml(string path, IEnumerable<State> states, int? startYear = null, int yearCount = 5, Cancel cancel = default)
    {
        await using var writer = File.CreateText(path);
        await ExportToXml(writer, states, startYear, yearCount, cancel);
    }

    static async Task ToXml(TextWriter writer, State state, IEnumerable<(Date start, Date end, string name)> holidays)
    {
        var settings = new XmlWriterSettings
        {
            Async = true,
            Indent = true
        };
        await using var xmlWriter = XmlWriter.Create(writer, settings);

        await xmlWriter.WriteStartDocumentAsync();
        await xmlWriter.WriteStartElementAsync(null, "SchoolHolidays", null);
        await xmlWriter.WriteAttributeStringAsync(null, "State", null, state.ToString());

        foreach (var (start, end, name) in holidays)
        {
            await xmlWriter.WriteStartElementAsync(null, "Holiday", null);
            await xmlWriter.WriteAttributeStringAsync(null, "Start", null, start.ToString("yyyy-MM-dd"));
            await xmlWriter.WriteAttributeStringAsync(null, "End", null, end.ToString("yyyy-MM-dd"));
            await xmlWriter.WriteAttributeStringAsync(null, "Name", null, name);
            await xmlWriter.WriteEndElementAsync();
        }

        await xmlWriter.WriteEndElementAsync();
        await xmlWriter.WriteEndDocumentAsync();
    }

    static async Task ToXmlMultiState(TextWriter writer, IEnumerable<(Date start, Date end, State state, string name)> holidays)
    {
        var settings = new XmlWriterSettings
        {
            Async = true,
            Indent = true
        };
        await using var xmlWriter = XmlWriter.Create(writer, settings);

        await xmlWriter.WriteStartDocumentAsync();
        await xmlWriter.WriteStartElementAsync(null, "SchoolHolidays", null);

        foreach (var (start, end, state, name) in holidays)
        {
            await xmlWriter.WriteStartElementAsync(null, "Holiday", null);
            await xmlWriter.WriteAttributeStringAsync(null, "Start", null, start.ToString("yyyy-MM-dd"));
            await xmlWriter.WriteAttributeStringAsync(null, "End", null, end.ToString("yyyy-MM-dd"));
            await xmlWriter.WriteAttributeStringAsync(null, "State", null, state.ToString());
            await xmlWriter.WriteAttributeStringAsync(null, "Name", null, name);
            await xmlWriter.WriteEndElementAsync();
        }

        await xmlWriter.WriteEndElementAsync();
        await xmlWriter.WriteEndDocumentAsync();
    }
}
