using System.Xml;

namespace AustralianHolidays;

public static partial class Holidays
{
    public static async Task<string> ExportToXml(int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToXml(TextWriter writer, int? startYear = null, int yearCount = 5)
    {
        var forYears = NationalForYears(startYear, yearCount);
        return ToXml(writer, null, forYears);
    }

    public static async Task<string> ExportToXml(State state, int? startYear = null, int yearCount = 5)
    {
        var builder = new StringBuilder();
        await using (var writer = new StringWriter(builder))
        {
            await ExportToXml(writer, state, startYear, yearCount);
        }

        return builder.ToString();
    }

    public static Task ExportToXml(TextWriter writer, State state, int? startYear = null, int yearCount = 5)
    {
        var forYears = ForYears(state, startYear, yearCount);
        return ToXml(writer, state, forYears);
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
