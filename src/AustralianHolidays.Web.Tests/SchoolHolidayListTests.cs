[TestFixture]
public class SchoolHolidayListTests
{
    [Test]
    public Task RendersPeriodsAndNoDataNote()
    {
        using var context = new BunitTestContext();
        var service = new SchoolHolidayFilterService(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

        var states = new HashSet<State> { State.NSW };
        // 2026 is covered; 2029 is not covered for NSW, so it should render the "no published dates" note.
        var years = new HashSet<int> { 2026, 2029 };
        var holidays = service.GetHolidays(states, years);

        var component = context.Render<SchoolHolidayList>(
            parameters => parameters
                .Add(_ => _.Holidays, holidays)
                .Add(_ => _.SelectedStates, states)
                .Add(_ => _.SelectedYears, years));

        return Verify(component.Markup);
    }
}
