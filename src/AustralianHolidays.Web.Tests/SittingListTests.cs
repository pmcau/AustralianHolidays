[TestFixture]
public class SittingListTests
{
    [Test]
    public Task RendersPeriodsEstimatesAndNoDataNote()
    {
        using var context = new BunitTestContext();
        var service = new SittingFilterService(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

        var chambers = new HashSet<Chamber> { Chamber.Senate };
        // 2026 is covered; 2029 is not, so it should render the "no published dates" note.
        var years = new HashSet<int> { 2026, 2029 };

        var component = context.Render<SittingList>(
            parameters => parameters
                .Add(_ => _.Periods, service.GetPeriods(chambers, years))
                .Add(_ => _.Estimates, service.GetEstimates(years))
                .Add(_ => _.SelectedChambers, chambers)
                .Add(_ => _.SelectedYears, years));

        return Verify(component.Markup);
    }

    [Test]
    public void PromptsWhenNothingSelected()
    {
        using var context = new BunitTestContext();

        var component = context.Render<SittingList>(
            parameters => parameters
                .Add(_ => _.SelectedChambers, new HashSet<Chamber>())
                .Add(_ => _.SelectedYears, new HashSet<int>()));

        That(component.Markup, Does.Contain("Select a chamber and a year"));
    }

    // The "next sitting" line trims the parts of the start date the end date repeats, so each branch of
    // that formatting needs a date that exercises it.
    [TestCase("2026-01-15", "19 to 20 Jan 2026", TestName = "NextSitting_SameMonth")]
    [TestCase("2026-06-26", "29 Jun to 2 Jul 2026", TestName = "NextSitting_SpansMonths")]
    [TestCase("2026-03-03", "sitting now, until 5 Mar 2026", TestName = "NextSitting_Underway")]
    public void NextSittingLine(string today, string expected)
    {
        using var context = new BunitTestContext();
        var date = Date.Parse(today, System.Globalization.CultureInfo.InvariantCulture);
        var service = new SittingFilterService(
            new FakeTimeProvider(new(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero)));

        var chambers = new HashSet<Chamber> { Chamber.House };
        var years = new HashSet<int> { 2026 };

        var component = context.Render<SittingList>(
            parameters => parameters
                .Add(_ => _.Periods, service.GetPeriods(chambers, years))
                .Add(_ => _.SelectedChambers, chambers)
                .Add(_ => _.SelectedYears, years));

        That(component.Find(".sitting-next").TextContent, Does.Contain(expected));
    }

    // Once the last sitting of the published calendar has passed there is nothing left to point at.
    [Test]
    public void NextSittingLineWhenCalendarExhausted()
    {
        using var context = new BunitTestContext();
        var service = new SittingFilterService(new FakeTimeProvider(new(2026, 12, 31, 0, 0, 0, TimeSpan.Zero)));

        var chambers = new HashSet<Chamber> { Chamber.House };
        var years = new HashSet<int> { 2026 };

        var component = context.Render<SittingList>(
            parameters => parameters
                .Add(_ => _.Periods, service.GetPeriods(chambers, years))
                .Add(_ => _.SelectedChambers, chambers)
                .Add(_ => _.SelectedYears, years));

        That(component.Find(".sitting-next").TextContent, Does.Contain("no further sittings published"));
    }

    // Estimates belong to the Senate, so they should not appear when only the House is in view.
    [Test]
    public void EstimatesHiddenWhenSenateNotSelected()
    {
        using var context = new BunitTestContext();
        var service = new SittingFilterService(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

        var chambers = new HashSet<Chamber> { Chamber.House };
        var years = new HashSet<int> { 2026 };

        var component = context.Render<SittingList>(
            parameters => parameters
                .Add(_ => _.Periods, service.GetPeriods(chambers, years))
                .Add(_ => _.Estimates, service.GetEstimates(years))
                .Add(_ => _.SelectedChambers, chambers)
                .Add(_ => _.SelectedYears, years));

        That(component.Markup, Does.Not.Contain("Senate estimates"));
    }
}
