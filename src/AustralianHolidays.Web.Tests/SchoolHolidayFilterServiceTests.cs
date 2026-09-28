public class SchoolHolidayFilterServiceTests
{
    // 15 January 2026 falls inside the 2026 summer break for every state.
    static SchoolHolidayFilterService CreateService() =>
        new(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

    [Test]
    public async Task ReturnsFourNamedPeriodsForCoveredYear()
    {
        var service = CreateService();

        var result = service.GetHolidays(
            new HashSet<State> { State.NSW },
            new HashSet<int> { 2026 });

        await Assert.That(result.Select(_ => _.Name))
            .IsEquivalentTo(["Summer", "Autumn", "Winter", "Spring"], CollectionOrdering.Matching);

        // The summer break is current on 15 Jan 2026; the later breaks are still upcoming.
        await Assert.That(result.Single(_ => _.Name == "Summer").TimeCategory).IsEqualTo(HolidayTimeCategory.Today);
        await Assert.That(result.Single(_ => _.Name == "Autumn").TimeCategory).IsEqualTo(HolidayTimeCategory.Future);
        await Assert.That(result.Single(_ => _.Name == "Spring").TimeCategory).IsEqualTo(HolidayTimeCategory.Future);
    }

    [Test]
    public async Task UncoveredYearReturnsEmpty()
    {
        var service = CreateService();

        var result = service.GetHolidays(
            new HashSet<State> { State.NSW },
            new HashSet<int> { 2029 });

        await Assert.That(result).IsEmpty();
    }

    [Test]
    public async Task AvailableYearsReflectCoverage()
    {
        await Assert.That(SchoolHolidayFilterService.GetAvailableYears(new HashSet<State> { State.NSW }))
            .IsEquivalentTo([2025, 2026, 2027], CollectionOrdering.Matching);

        var allStates = new HashSet<State>(Enum.GetValues<State>());
        var years = SchoolHolidayFilterService.GetAvailableYears(allStates);
        await Assert.That(years[0]).IsEqualTo(2025);
        await Assert.That(years[^1]).IsEqualTo(2030);
    }

    [Test]
    public async Task DefaultYearIsCurrentWhenCovered()
    {
        var service = CreateService();

        await Assert.That(service.GetDefaultYear(new HashSet<State> { State.NSW })!.Value).IsEqualTo(2026);
    }
}
