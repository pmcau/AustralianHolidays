using Microsoft.JSInterop;

namespace AustralianHolidays.Web.Services;

public class FileDownloadService(IJSRuntime jsRuntime)
{
    public async Task DownloadJsonAsync(State? state, int startYear, int yearCount)
    {
        var content = state.HasValue
            ? await Holidays.ExportToJson(state.Value, startYear, yearCount)
            : await Holidays.ExportToJson(startYear, yearCount);

        var filename = GetFilename("holidays", state, "json");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "application/json", content);
    }

    public async Task DownloadCsvAsync(State? state, int startYear, int yearCount)
    {
        var content = state.HasValue
            ? await Holidays.ExportToCsv(state.Value, startYear, yearCount)
            : await Holidays.ExportToCsv(startYear, yearCount);

        var filename = GetFilename("holidays", state, "csv");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "text/csv", content);
    }

    public async Task DownloadXmlAsync(State? state, int startYear, int yearCount)
    {
        var content = state.HasValue
            ? await Holidays.ExportToXml(state.Value, startYear, yearCount)
            : await Holidays.ExportToXml(startYear, yearCount);

        var filename = GetFilename("holidays", state, "xml");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "application/xml", content);
    }

    public async Task DownloadMarkdownAsync(State? state, int startYear, int yearCount)
    {
        var content = state.HasValue
            ? await Holidays.ExportToMarkdown(state.Value, startYear, yearCount)
            : await Holidays.ExportToMarkdown(startYear, yearCount);

        var filename = GetFilename("holidays", state, "md");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "text/markdown", content);
    }

    public async Task DownloadIcsAsync(State? state, int startYear, int yearCount)
    {
        var content = state.HasValue
            ? await Holidays.ExportToIcs(state.Value, startYear, yearCount)
            : await Holidays.ExportToIcs(startYear, yearCount);

        var filename = GetFilename("holidays", state, "ics");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "text/calendar", content);
    }

    public async Task DownloadExcelAsync(State? state, int startYear, int yearCount)
    {
        var bytes = state.HasValue
            ? await Holidays.ExportToExcel(state.Value, startYear, yearCount)
            : await Holidays.ExportToExcel(startYear, yearCount);

        var base64 = Convert.ToBase64String(bytes);
        var filename = GetFilename("holidays", state, "xlsx");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadBlob", filename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64);
    }

    static string GetFilename(string baseName, State? state, string extension)
    {
        var statePart = state.HasValue ? $"-{state.Value}" : "-national";
        return $"{baseName}{statePart}.{extension}";
    }
}
