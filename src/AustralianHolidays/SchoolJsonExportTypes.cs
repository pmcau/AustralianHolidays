using System.Text.Json.Serialization;

namespace AustralianHolidays;

internal record SchoolHolidayJson(string start, string end, string name);

internal record SchoolHolidayExportJson(string state, IEnumerable<SchoolHolidayJson> holidays);

internal record MultiStateSchoolHolidayJson(string start, string end, string state, string name);

[JsonSerializable(typeof(SchoolHolidayExportJson))]
[JsonSerializable(typeof(IEnumerable<MultiStateSchoolHolidayJson>))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal partial class SchoolHolidayJsonContext : JsonSerializerContext;
