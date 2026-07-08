[TestFixture]
public class SchoolHolidayFilterServiceTests
{
    // 15 January 2026 falls inside the 2026 summer break for every state.
    static SchoolHolidayFilterService CreateService() =>
        new(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

    [Test]
    public void ReturnsFourNamedPeriodsForCoveredYear()
    {
        var service = CreateService();

        var result = service.GetHolidays(
            new HashSet<State> { State.NSW },
            new HashSet<int> { 2026 });

        That(
            result.Select(_ => _.Name),
            Is.EqualTo(["Summer", "Autumn", "Winter", "Spring"]));

        // The summer break is current on 15 Jan 2026; the later breaks are still upcoming.
        AreEqual(HolidayTimeCategory.Today, result.Single(_ => _.Name == "Summer").TimeCategory);
        AreEqual(HolidayTimeCategory.Future, result.Single(_ => _.Name == "Autumn").TimeCategory);
        AreEqual(HolidayTimeCategory.Future, result.Single(_ => _.Name == "Spring").TimeCategory);
    }

    [Test]
    public void UncoveredYearReturnsEmpty()
    {
        var service = CreateService();

        var result = service.GetHolidays(
            new HashSet<State> { State.NSW },
            new HashSet<int> { 2029 });

        IsEmpty(result);
    }

    [Test]
    public void AvailableYearsReflectCoverage()
    {
        That(
            SchoolHolidayFilterService.GetAvailableYears(new HashSet<State> { State.NSW }),
            Is.EqualTo([2025, 2026, 2027]));

        var allStates = new HashSet<State>(Enum.GetValues<State>());
        var years = SchoolHolidayFilterService.GetAvailableYears(allStates);
        AreEqual(2025, years[0]);
        AreEqual(2030, years[^1]);
    }

    [Test]
    public void DefaultYearIsCurrentWhenCovered()
    {
        var service = CreateService();

        AreEqual(2026, service.GetDefaultYear(new HashSet<State> { State.NSW })!.Value);
    }
}
