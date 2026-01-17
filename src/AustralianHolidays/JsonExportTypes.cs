using System.Text.Json.Serialization;

namespace AustralianHolidays;

internal record HolidayJson(string date, string name);

internal record HolidayExportJson(string state, IEnumerable<HolidayJson> holidays);

internal record MultiStateHolidayJson(string date, string state, string name);

[JsonSerializable(typeof(HolidayExportJson))]
[JsonSerializable(typeof(IEnumerable<MultiStateHolidayJson>))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal partial class HolidayJsonContext : JsonSerializerContext;
