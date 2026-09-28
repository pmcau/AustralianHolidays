public class HolidayListTests : BunitTestContext
{
    [Test]
    public async Task EmptyList_ShowsEmptyState()
    {
        var cut = Render<HolidayList>(_ => _
            .Add(_ => _.Holidays, [])
            .Add(_ => _.ShowStateColumn, false));

        var emptyState = cut.Find(".empty-state");
        await Assert.That(emptyState).IsNotNull();
        await Assert.That(emptyState.TextContent).Contains("No holidays found");
    }

    [Test]
    public async Task NullHolidays_ShowsEmptyState()
    {
        var cut = Render<HolidayList>(_ => _
            .Add(_ => _.Holidays, null)
            .Add(_ => _.ShowStateColumn, false));

        var emptyState = cut.Find(".empty-state");
        await Assert.That(emptyState).IsNotNull();
    }

    [Test]
    public async Task WithHolidays_ShowsTable()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new(2025, 1, 1), "New Year's Day", [State.NSW], HolidayTimeCategory.Past),
            new(new(2025, 1, 27), "Australia Day", [State.NSW], HolidayTimeCategory.Past)
        };

        var cut = Render<HolidayList>(_ => _
            .Add(_ => _.Holidays, holidays)
            .Add(_ => _.ShowStateColumn, false));

        var table = cut.Find(".holiday-table");
        await Assert.That(table).IsNotNull();

        var rows = cut.FindAll("tbody tr");
        await Assert.That(rows.Count).IsEqualTo(2);
    }

    [Test]
    public async Task ShowStateColumn_DisplaysStateColumn()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new(2025, 1, 1), "New Year's Day", [State.NSW], HolidayTimeCategory.Past)
        };

        var cut = Render<HolidayList>(_ => _
            .Add(_ => _.Holidays, holidays)
            .Add(_ => _.ShowStateColumn, true));

        var headers = cut.FindAll("thead th");
        await Assert.That(headers.Count).IsEqualTo(4);

        var stateBadge = cut.Find(".state-badge");
        await Assert.That(stateBadge.TextContent).IsEqualTo("NSW");
    }

    [Test]
    public async Task ShowStateColumn_DisplaysMultipleStateBadges()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new(2025, 1, 26), "Australia Day", [State.NSW, State.VIC, State.QLD], HolidayTimeCategory.Past)
        };

        var cut = Render<HolidayList>(_ => _
            .Add(_ => _.Holidays, holidays)
            .Add(_ => _.ShowStateColumn, true));

        var stateBadges = cut.FindAll(".state-badge");
        await Assert.That(stateBadges.Count).IsEqualTo(3);
        await Assert.That(stateBadges[0].TextContent).IsEqualTo("NSW");
        await Assert.That(stateBadges[1].TextContent).IsEqualTo("VIC");
        await Assert.That(stateBadges[2].TextContent).IsEqualTo("QLD");
    }

    [Test]
    public async Task HideStateColumn_NoStateColumn()
    {
        var holidays = new List<HolidayViewModel>
        {
            new(new(2025, 1, 1), "New Year's Day", [State.NSW], HolidayTimeCategory.Past)
        };

        var cut = Render<HolidayList>(_ => _
            .Add(_ => _.Holidays, holidays)
            .Add(_ => _.ShowStateColumn, false));

        var headers = cut.FindAll("thead th");
        await Assert.That(headers.Count).IsEqualTo(3);
    }
}
