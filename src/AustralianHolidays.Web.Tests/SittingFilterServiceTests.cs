[TestFixture]
public class SittingFilterServiceTests
{
    // 15 January 2026 sits just before the 19-20 January recall, so the recall is the next sitting.
    static SittingFilterService CreateService() =>
        new(new FakeTimeProvider(new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero)));

    [Test]
    public void ReturnsNamedBlocksForCoveredYear()
    {
        var service = CreateService();

        var result = service.GetPeriods(
            new HashSet<Chamber> { Chamber.House },
            new HashSet<int> { 2026 });

        That(
            result.Select(_ => _.Name).Distinct(),
            Is.EqualTo(["Recall", "Autumn", "Winter", "Spring"]));

        // Nothing has happened yet on 15 Jan 2026, so every period is still ahead.
        That(result.Select(_ => _.TimeCategory), Is.All.EqualTo(HolidayTimeCategory.Future));
    }

    // The House sits alone during estimates weeks, so those periods must be tagged House only.
    [Test]
    public void ChambersTaggedOnlyWhenTheySitEveryDay()
    {
        var service = CreateService();

        var result = service.GetPeriods(
            new HashSet<Chamber> { Chamber.House },
            new HashSet<int> { 2026 });

        var shared = result.Single(_ => _.Start == new Date(2026, 3, 2));
        That(shared.SittingChambers, Is.EqualTo([Chamber.House, Chamber.Senate]));

        var houseOnly = result.Single(_ => _.Start == new Date(2026, 2, 9));
        That(houseOnly.SittingChambers, Is.EqualTo([Chamber.House]));

        // A Senate-only week is tagged Senate even when read from the Senate table.
        var senateOnly = service
            .GetPeriods(new HashSet<Chamber> { Chamber.Senate }, new HashSet<int> { 2026 })
            .Single(_ => _.Start == new Date(2026, 11, 16));
        That(senateOnly.SittingChambers, Is.EqualTo([Chamber.Senate]));
    }

    [Test]
    public void EstimatesAreSeparateFromSittings()
    {
        var service = CreateService();

        var estimates = service.GetEstimates(new HashSet<int> { 2026 });

        That(
            estimates.Select(_ => _.Name),
            Is.EqualTo(["Additional", "Budget", "Budget", "Supplementary Budget"]));

        // An estimates round is not a Senate sitting period.
        var senate = service.GetPeriods(
            new HashSet<Chamber> { Chamber.Senate },
            new HashSet<int> { 2026 });
        IsFalse(senate.Any(_ => _.Start == new Date(2026, 2, 9)));
    }

    [Test]
    public void UncoveredYearReturnsEmpty()
    {
        var service = CreateService();

        IsEmpty(
            service.GetPeriods(
                new HashSet<Chamber> { Chamber.House },
                new HashSet<int> { 2029 }));
        IsEmpty(service.GetEstimates(new HashSet<int> { 2029 }));
    }

    [Test]
    public void AvailableYearsReflectCoverage()
    {
        That(
            SittingFilterService.GetAvailableYears(new HashSet<Chamber> { Chamber.House }),
            Is.EqualTo([2026]));
        IsEmpty(SittingFilterService.GetAvailableYears(new HashSet<Chamber>()));
    }

    [Test]
    public void DefaultYearIsCurrentWhenCovered()
    {
        var service = CreateService();

        AreEqual(2026, service.GetDefaultYear(new HashSet<Chamber> { Chamber.House })!.Value);
        IsNull(service.GetDefaultYear(new HashSet<Chamber>()));
    }
}
