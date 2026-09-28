public class SittingFilterServiceTests
{
    // 15 January 2026 sits just before the 19-20 January recall, so the recall is the next sitting.
    static SittingFilterService CreateService() =>
        new(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

    [Test]
    public async Task ReturnsNamedBlocksForCoveredYear()
    {
        var service = CreateService();

        var result = service.GetPeriods(
            new HashSet<Chamber> { Chamber.House },
            new HashSet<int> { 2026 });

        await Assert.That(result.Select(_ => _.Name).Distinct())
            .IsEquivalentTo(["Recall", "Autumn", "Winter", "Spring"], CollectionOrdering.Matching);

        // Nothing has happened yet on 15 Jan 2026, so every period is still ahead.
        await Assert.That(result.Select(_ => _.TimeCategory)).All().Satisfy(_ => _.IsEqualTo(HolidayTimeCategory.Future));
    }

    // The House sits alone during estimates weeks, so those periods must be tagged House only.
    [Test]
    public async Task ChambersTaggedOnlyWhenTheySitEveryDay()
    {
        var service = CreateService();

        var result = service.GetPeriods(
            new HashSet<Chamber> { Chamber.House },
            new HashSet<int> { 2026 });

        var shared = result.Single(_ => _.Start == new Date(2026, 3, 2));
        await Assert.That(shared.SittingChambers).IsEquivalentTo([Chamber.House, Chamber.Senate], CollectionOrdering.Matching);

        var houseOnly = result.Single(_ => _.Start == new Date(2026, 2, 9));
        await Assert.That(houseOnly.SittingChambers).IsEquivalentTo([Chamber.House], CollectionOrdering.Matching);

        // A Senate-only week is tagged Senate even when read from the Senate table.
        var senateOnly = service
            .GetPeriods(new HashSet<Chamber> { Chamber.Senate }, new HashSet<int> { 2026 })
            .Single(_ => _.Start == new Date(2026, 11, 16));
        await Assert.That(senateOnly.SittingChambers).IsEquivalentTo([Chamber.Senate], CollectionOrdering.Matching);
    }

    [Test]
    public async Task EstimatesAreSeparateFromSittings()
    {
        var service = CreateService();

        var estimates = service.GetEstimates(new HashSet<int> { 2026 });

        await Assert.That(estimates.Select(_ => _.Name))
            .IsEquivalentTo(["Additional", "Budget", "Budget", "Supplementary Budget"], CollectionOrdering.Matching);

        // An estimates round is not a Senate sitting period.
        var senate = service.GetPeriods(
            new HashSet<Chamber> { Chamber.Senate },
            new HashSet<int> { 2026 });
        await Assert.That(senate.Any(_ => _.Start == new Date(2026, 2, 9))).IsFalse();
    }

    [Test]
    public async Task UncoveredYearReturnsEmpty()
    {
        var service = CreateService();

        await Assert.That(
            service.GetPeriods(
                new HashSet<Chamber> { Chamber.House },
                new HashSet<int> { 2029 }))
            .IsEmpty();
        await Assert.That(service.GetEstimates(new HashSet<int> { 2029 })).IsEmpty();
    }

    [Test]
    public async Task AvailableYearsReflectCoverage()
    {
        await Assert.That(SittingFilterService.GetAvailableYears(new HashSet<Chamber> { Chamber.House }))
            .IsEquivalentTo([2026], CollectionOrdering.Matching);
        await Assert.That(SittingFilterService.GetAvailableYears(new HashSet<Chamber>())).IsEmpty();
    }

    [Test]
    public async Task DefaultYearIsCurrentWhenCovered()
    {
        var service = CreateService();

        await Assert.That(service.GetDefaultYear(new HashSet<Chamber> { Chamber.House })!.Value).IsEqualTo(2026);
        await Assert.That(service.GetDefaultYear(new HashSet<Chamber>())).IsNull();
    }
}
