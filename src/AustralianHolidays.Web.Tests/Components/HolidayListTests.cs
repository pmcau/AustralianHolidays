using AustralianHolidays.Web.Components;

namespace AustralianHolidays.Web.Tests.Components;

[TestFixture]
public class HolidayListTests : BunitTestContext
{
    [Test]
    public void EmptyList_ShowsEmptyState()
    {
        var cut = Render<HolidayList>(parameters => parameters
            .Add(p => p.Holidays, new List<HolidayViewModel>())
            .Add(p => p.ShowStateColumn, false));

        var emptyState = cut.Find(".empty-state");
        That(emptyState, Is.Not.Null);
        That(emptyState.TextContent, Does.Contain("No holidays found"));
    }

    [Test]
    public void NullHolidays_ShowsEmptyState()
    {
        var cut = Render<HolidayList>(parameters => parameters
            .Add(p => p.Holidays, null)
            .Add(p => p.ShowStateColumn, false));

        var emptyState = cut.Find(".empty-state");
        That(emptyState, Is.Not.Null);
    }

    [Test]
    public void WithHolidays_ShowsTable()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new Date(2025, 1, 1), "New Year's Day", State.NSW),
            new(new Date(2025, 1, 27), "Australia Day", State.NSW)
        };

        var cut = Render<HolidayList>(parameters => parameters
            .Add(p => p.Holidays, holidays)
            .Add(p => p.ShowStateColumn, false));

        var table = cut.Find(".holiday-table");
        That(table, Is.Not.Null);

        var rows = cut.FindAll("tbody tr");
        That(rows.Count, Is.EqualTo(2));
    }

    [Test]
    public void ShowStateColumn_DisplaysStateColumn()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new Date(2025, 1, 1), "New Year's Day", State.NSW)
        };

        var cut = Render<HolidayList>(parameters => parameters
            .Add(p => p.Holidays, holidays)
            .Add(p => p.ShowStateColumn, true));

        var headers = cut.FindAll("thead th");
        That(headers.Count, Is.EqualTo(4));

        var stateBadge = cut.Find(".state-badge");
        That(stateBadge.TextContent, Is.EqualTo("NSW"));
    }

    [Test]
    public void HideStateColumn_NoStateColumn()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new Date(2025, 1, 1), "New Year's Day", State.NSW)
        };

        var cut = Render<HolidayList>(parameters => parameters
            .Add(p => p.Holidays, holidays)
            .Add(p => p.ShowStateColumn, false));

        var headers = cut.FindAll("thead th");
        That(headers.Count, Is.EqualTo(3));
    }
}
