public class FileDownloadService(IJSRuntime jsRuntime)
{
    public async Task DownloadJsonAsync(IReadOnlySet<State> states, int startYear, int yearCount)
    {
        var content = states.Count == 1
            ? await Holidays.ExportToJson(states.First(), startYear, yearCount)
            : await Holidays.ExportToJson(states, startYear, yearCount);

        var filename = GetFilename("holidays", states, "json");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "application/json", content);
    }

    public async Task DownloadCsvAsync(IReadOnlySet<State> states, int startYear, int yearCount)
    {
        var content = states.Count == 1
            ? await Holidays.ExportToCsv(states.First(), startYear, yearCount)
            : await Holidays.ExportToCsv(states, startYear, yearCount);

        var filename = GetFilename("holidays", states, "csv");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "text/csv", content);
    }

    public async Task DownloadXmlAsync(IReadOnlySet<State> states, int startYear, int yearCount)
    {
        var content = states.Count == 1
            ? await Holidays.ExportToXml(states.First(), startYear, yearCount)
            : await Holidays.ExportToXml(states, startYear, yearCount);

        var filename = GetFilename("holidays", states, "xml");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "application/xml", content);
    }

    public async Task DownloadMarkdownAsync(IReadOnlySet<State> states, int startYear, int yearCount)
    {
        var content = states.Count == 1
            ? await Holidays.ExportToMarkdown(states.First(), startYear, yearCount)
            : await Holidays.ExportToMarkdown(states, startYear, yearCount);

        var filename = GetFilename("holidays", states, "md");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "text/markdown", content);
    }

    public async Task DownloadIcsAsync(IReadOnlySet<State> states, int startYear, int yearCount)
    {
        var content = states.Count == 1
            ? await Holidays.ExportToIcs(states.First(), startYear, yearCount)
            : await Holidays.ExportToIcs(states, startYear, yearCount);

        var filename = GetFilename("holidays", states, "ics");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadFile", filename, "text/calendar", content);
    }

    public async Task DownloadExcelAsync(IReadOnlySet<State> states, int startYear, int yearCount)
    {
        var bytes = states.Count == 1
            ? await Holidays.ExportToExcel(states.First(), startYear, yearCount)
            : await Holidays.ExportToExcel(states, startYear, yearCount);

        var base64 = Convert.ToBase64String(bytes);
        var filename = GetFilename("holidays", states, "xlsx");
        await jsRuntime.InvokeVoidAsync("fileDownload.downloadBlob", filename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64);
    }

    static string GetFilename(string baseName, IReadOnlySet<State> states, string extension)
    {
        var allStates = Enum.GetValues<State>();

        string statePart;
        if (states.Count == 0 || states.Count == allStates.Length)
        {
            statePart = "-all";
        }
        else if (states.Count == 1)
        {
            statePart = $"-{states.First()}";
        }
        else
        {
            statePart = "-" + string.Join("-", states.OrderBy(s => s));
        }

        return $"{baseName}{statePart}.{extension}";
    }
}
