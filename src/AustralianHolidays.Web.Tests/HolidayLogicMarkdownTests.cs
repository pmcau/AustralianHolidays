using Markdig;

namespace AustralianHolidays.Web.Tests;

public class HolidayLogicMarkdownTests
{
    static readonly string RepoRoot = Path.GetFullPath(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));
    static readonly string MarkdownFile = Path.Combine(RepoRoot, "holiday-logic.include.md");
    static readonly string WwwrootPath = Path.Combine(RepoRoot, "src", "AustralianHolidays.Web", "wwwroot");
    static readonly string HolidayLogicHtml = Path.Combine(WwwrootPath, "holiday-logic.html");
    static readonly string HolidayLogicCollapsibleHtml = Path.Combine(WwwrootPath, "holiday-logic-collapsible.html");
    const string SplitMarker = "## Public Holiday Substitution Rules";

    [Test]
    public async Task ConvertMarkdownToHtml()
    {
        var markdown = await File.ReadAllTextAsync(MarkdownFile);
        var splitIndex = markdown.IndexOf(SplitMarker);

        string alwaysVisibleHtml;
        string collapsibleHtml;

        if (splitIndex > 0)
        {
            var alwaysVisibleMd = markdown[..splitIndex];
            var collapsibleMd = markdown[(splitIndex + SplitMarker.Length)..];

            alwaysVisibleHtml = Markdown.ToHtml(alwaysVisibleMd);
            collapsibleHtml = Markdown.ToHtml(collapsibleMd);
        }
        else
        {
            alwaysVisibleHtml = Markdown.ToHtml(markdown);
            collapsibleHtml = "";
        }

        await File.WriteAllTextAsync(HolidayLogicHtml, alwaysVisibleHtml);
        await File.WriteAllTextAsync(HolidayLogicCollapsibleHtml, collapsibleHtml);

        await Verify(new { alwaysVisibleHtml, collapsibleHtml });
    }
}
