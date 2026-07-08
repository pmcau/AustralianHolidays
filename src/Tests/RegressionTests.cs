[TestFixture]
public class RegressionTests
{
    // In 2038 Easter Sunday falls on 25 April, which is also Anzac Day. Building the year used to
    // throw a duplicate-key ArgumentException; via the static cache pre-population (current year
    // ± range) that would have bricked the whole library once the clock reached 2028. The two
    // holidays must now be merged onto the single date instead.
    [Test]
    public void EasterSundayOnAnzacDayMergesInsteadOfThrowing()
    {
        var date = new Date(2038, 4, 25);
        foreach (var state in Enum.GetValues<State>())
        {
            IsTrue(date.IsHoliday(state, out var name));
            That(name, Does.Contain("Anzac Day"));
            That(name, Does.Contain("Easter Sunday"));
        }

        IsTrue(Holidays.ForNational(2038).TryGetValue(date, out var national));
        That(national, Does.Contain("Anzac Day"));
        That(national, Does.Contain("Easter Sunday"));
    }

    // The secondary collision the merge also resolves: Easter Monday (26 April 2038) lands on the
    // substitute day NSW grants because Anzac Day fell on a Sunday.
    [Test]
    public void EasterMondayOnAnzacSubstituteMerges()
    {
        IsTrue(new Date(2038, 4, 26).IsNswHoliday(out var name));
        That(name, Does.Contain("Easter Monday"));
        That(name, Does.Contain("Anzac Day (additional)"));
    }

    // Dates in the January tail of a shutdown used to return false because the period was anchored
    // on the date's own year instead of the previous December.
    [Test]
    public void IsFederalGovernmentShutdownCoversJanuary()
    {
        // 2025 -> 2026 shutdown: 25 Dec 2025 .. 1 Jan 2026 (New Year's Day, a Thursday).
        IsTrue(new Date(2025, 12, 25).IsFederalGovernmentShutdown());
        IsTrue(new Date(2025, 12, 31).IsFederalGovernmentShutdown());
        IsTrue(new Date(2026, 1, 1).IsFederalGovernmentShutdown());
        IsFalse(new Date(2026, 1, 2).IsFederalGovernmentShutdown());

        // 2027 -> 2028 shutdown: New Year's Day 2028 is a Saturday, so the holiday shifts to Mon 3 Jan.
        IsTrue(new Date(2028, 1, 1).IsFederalGovernmentShutdown());
        IsTrue(new Date(2028, 1, 3).IsFederalGovernmentShutdown());
        IsFalse(new Date(2028, 1, 4).IsFederalGovernmentShutdown());

        // Well clear of any shutdown.
        IsFalse(new Date(2026, 6, 15).IsFederalGovernmentShutdown());
    }

    // The Is* check must agree with the boundaries reported by GetFederalGovernmentShutdown.
    [Test]
    public void IsFederalGovernmentShutdownMatchesReportedRange()
    {
        var (start, end) = Holidays.GetFederalGovernmentShutdown(2025);
        IsTrue(start.IsFederalGovernmentShutdown());
        IsTrue(end.IsFederalGovernmentShutdown());
    }

    // Single-state ICS export used to emit identical UIDs for repeated same-name holidays. NSW 2021
    // has Christmas Day on a Saturday plus Boxing Day on a Sunday, producing two
    // "Christmas (additional)" entries whose UIDs must still be distinct.
    [Test]
    public async Task SingleStateIcsUidsAreUnique()
    {
        var ics = await Holidays.ExportToIcs(State.NSW, 2021, 1);
        var uids = ics.Split('\n')
            .Where(_ => _.StartsWith("UID:", StringComparison.Ordinal))
            .Select(_ => _.Trim())
            .ToList();

        var christmasAdditional = uids.Count(_ => _.Contains("Christmas (additional)", StringComparison.Ordinal));
        AreEqual(2, christmasAdditional);
        AreEqual(uids.Count, uids.Distinct().Count());
    }
}
