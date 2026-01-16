using System.Xml;

namespace AustralianHolidays;

public static partial class Holidays
{
    /// <summary>
    /// Exports national public holidays to XML format.
    /// </summary>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the XML-formatted holiday data.</returns>
    public static async Task<string> ExportToXml(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports national public holidays to XML format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the XML output to.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToXml(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToXml(writer, null, forYears);
    }

    /// <summary>
    /// Exports public holidays for a specific state to XML format.
    /// </summary>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    /// <returns>A string containing the XML-formatted holiday data.</returns>
    public static async Task<string> ExportToXml(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exports public holidays for a specific state to XML format, writing to a TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to write the XML output to.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static Task ExportToXml(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToXml(writer, state, forYears);
    }

    /// <summary>
    /// Exports national public holidays to an XML file.
    /// </summary>
    /// <param name="path">The file path where the XML data will be written.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToXml(string path, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToXml(writer, startYear, yearCount);
    }

    /// <summary>
    /// Exports public holidays for a specific state to an XML file.
    /// </summary>
    /// <param name="path">The file path where the XML data will be written.</param>
    /// <param name="state">The Australian state to export holidays for.</param>
    /// <param name="startYear">The starting year for the export. If null, uses the current year.</param>
    /// <param name="yearCount">The number of years to include in the export. Default is 5.</param>
    public static async Task ExportToXml(string path, State state, int? startYear = null, int yearCount = 5)
    {
        await using var writer = File.CreateText(path);
        await ExportToXml(writer, state, startYear, yearCount);
    }

    static async Task ToXml(TextWriter writer, State? state, IOrderedEnumerable<(Date date, string name)> forYears)
    {
        var settings = new XmlWriterSettings {Async = true, Indent = true};
        await using var xmlWriter = XmlWriter.Create(writer, settings);

        await xmlWriter.WriteStartDocumentAsync();
        await xmlWriter.WriteStartElementAsync(null, "Holidays", null);
        await xmlWriter.WriteAttributeStringAsync(null, "State", null, state?.ToString() ?? "National");

        foreach (var (date, name) in forYears)
        {
            await xmlWriter.WriteStartElementAsync(null, "Holiday", null);
            await xmlWriter.WriteAttributeStringAsync(null, "Date", null, date.ToString("yyyy-MM-dd"));
            await xmlWriter.WriteAttributeStringAsync(null, "Name", null, name);
            await xmlWriter.WriteEndElementAsync();
        }

        await xmlWriter.WriteEndElementAsync();
        await xmlWriter.WriteEndDocumentAsync();
    }
}
