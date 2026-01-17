using Markdig;

namespace AustralianHolidays.Web.Tests;

public class HolidayLogicMarkdownTests
{
    static readonly string markdownFile = Path.Combine(ProjectFiles.SolutionDirectory,"../", "holiday-logic.include.md");
    static readonly string wwwrootPath = Path.Combine(ProjectFiles.SolutionDirectory, "AustralianHolidays.Web", "wwwroot");
    static readonly string holidayLogicHtml = Path.Combine(wwwrootPath, "holiday-logic.html");
    static readonly string holidayLogicCollapsibleHtml = Path.Combine(wwwrootPath, "holiday-logic-collapsible.html");
    const string splitMarker = "## Public Holiday Substitution Rules";

    [Test]
    public async Task ConvertMarkdownToHtml()
    {
        var markdown = await File.ReadAllTextAsync(markdownFile);
        var splitIndex = markdown.IndexOf(splitMarker, StringComparison.Ordinal);

        string alwaysVisibleHtml;
        string collapsibleHtml;

        if (splitIndex > 0)
        {
            var alwaysVisibleMd = markdown[..splitIndex];
            var collapsibleMd = markdown[(splitIndex + splitMarker.Length)..];

            alwaysVisibleHtml = Markdown.ToHtml(alwaysVisibleMd);
            collapsibleHtml = Markdown.ToHtml(collapsibleMd);
        }
        else
        {
            alwaysVisibleHtml = Markdown.ToHtml(markdown);
            collapsibleHtml = "";
        }

        await File.WriteAllTextAsync(holidayLogicHtml, alwaysVisibleHtml);
        await File.WriteAllTextAsync(holidayLogicCollapsibleHtml, collapsibleHtml);

        await Verify(new { alwaysVisibleHtml, collapsibleHtml });
    }
}
