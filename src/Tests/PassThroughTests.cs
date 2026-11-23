// Per-state helpers, parliament shortcuts and the service classes are thin wrappers over the
// snapshotted core API. These tests pin each wrapper to the member it should delegate to, which
// catches a wrapper pointing at the wrong state or chamber.
public class PassThroughTests
{
    delegate bool OutCheck(Date date, [NotNullWhen(true)] out string? name);

    record StateChecks(State State, Func<Date, bool> Is, OutCheck IsWithName, Func<int, IReadOnlyDictionary<Date, string>> For);

    static IEnumerable<Date> Days(int startYear, int yearCount)
    {
        var end = new Date(startYear + yearCount, 1, 1);
        for (var date = new Date(startYear, 1, 1); date < end; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    static StateChecks[] StaticChecks() =>
    [
        new(State.ACT, Holidays.IsActHoliday, Holidays.IsActHoliday, Holidays.ForAct),
        new(State.NSW, Holidays.IsNswHoliday, Holidays.IsNswHoliday, Holidays.ForNsw),
        new(State.NT, Holidays.IsNtHoliday, Holidays.IsNtHoliday, Holidays.ForNt),
        new(State.QLD, Holidays.IsQldHoliday, Holidays.IsQldHoliday, Holidays.ForQld),
        new(State.SA, Holidays.IsSaHoliday, Holidays.IsSaHoliday, Holidays.ForSa),
        new(State.TAS, Holidays.IsTasHoliday, Holidays.IsTasHoliday, Holidays.ForTas),
        new(State.VIC, Holidays.IsVicHoliday, Holidays.IsVicHoliday, Holidays.ForVic),
        new(State.WA, Holidays.IsWaHoliday, Holidays.IsWaHoliday, Holidays.ForWa),
    ];

    static StateChecks[] ServiceChecks(HolidayService service) =>
    [
        new(State.ACT, service.IsActHoliday, service.IsActHoliday, service.ForAct),
        new(State.NSW, service.IsNswHoliday, service.IsNswHoliday, service.ForNsw),
        new(State.NT, service.IsNtHoliday, service.IsNtHoliday, service.ForNt),
        new(State.QLD, service.IsQldHoliday, service.IsQldHoliday, service.ForQld),
        new(State.SA, service.IsSaHoliday, service.IsSaHoliday, service.ForSa),
        new(State.TAS, service.IsTasHoliday, service.IsTasHoliday, service.ForTas),
        new(State.VIC, service.IsVicHoliday, service.IsVicHoliday, service.ForVic),
        new(State.WA, service.IsWaHoliday, service.IsWaHoliday, service.ForWa),
    ];

    static async Task AssertMatchesCore(StateChecks[] checks)
    {
        foreach (var check in checks)
        {
            foreach (var date in Days(2025, 2))
            {
                var expected = date.IsHoliday(check.State, out var expectedName);
                await Assert.That(check.Is(date)).IsEqualTo(expected);
                await Assert.That(check.IsWithName(date, out var name)).IsEqualTo(expected);
                await Assert.That(name).IsEqualTo(expectedName);
            }

            await Assert.That(check.For(2025)).IsEquivalentTo(Holidays.ForYears(check.State, 2025).ToDictionary(_ => _.date, _ => _.name));
        }
    }

    [Test]
    public Task StateHolidays() =>
        AssertMatchesCore(StaticChecks());

    [Test]
    public async Task NationalHoliday()
    {
        var national = Holidays.ForNational(2025);
        await Assert.That(national).IsNotEmpty();
        foreach (var date in Days(2025, 1))
        {
            var expected = national.TryGetValue(date, out var expectedName);
            await Assert.That(date.IsNationalHoliday()).IsEqualTo(expected);
            await Assert.That(date.IsNationalHoliday(out var name)).IsEqualTo(expected);
            await Assert.That(name).IsEqualTo(expectedName);
        }
    }

    [Test]
    public async Task StateSchoolHolidays()
    {
        (State state, Func<Date, bool> isHoliday, OutCheck isHolidayWithName)[] checks =
        [
            (State.ACT, SchoolHolidays.IsActSchoolHoliday, SchoolHolidays.IsActSchoolHoliday),
            (State.NSW, SchoolHolidays.IsNswSchoolHoliday, SchoolHolidays.IsNswSchoolHoliday),
            (State.NT, SchoolHolidays.IsNtSchoolHoliday, SchoolHolidays.IsNtSchoolHoliday),
            (State.QLD, SchoolHolidays.IsQldSchoolHoliday, SchoolHolidays.IsQldSchoolHoliday),
            (State.SA, SchoolHolidays.IsSaSchoolHoliday, SchoolHolidays.IsSaSchoolHoliday),
            (State.TAS, SchoolHolidays.IsTasSchoolHoliday, SchoolHolidays.IsTasSchoolHoliday),
            (State.VIC, SchoolHolidays.IsVicSchoolHoliday, SchoolHolidays.IsVicSchoolHoliday),
            (State.WA, SchoolHolidays.IsWaSchoolHoliday, SchoolHolidays.IsWaSchoolHoliday),
        ];

        foreach (var (state, isHoliday, isHolidayWithName) in checks)
        {
            foreach (var date in Days(2025, 2))
            {
                var expected = date.IsSchoolHoliday(state, out var expectedName);
                await Assert.That(isHoliday(date)).IsEqualTo(expected);
                await Assert.That(isHolidayWithName(date, out var name)).IsEqualTo(expected);
                await Assert.That(name).IsEqualTo(expectedName);
            }
        }
    }

    [Test]
    public async Task ChamberSittingDays()
    {
        (Chamber chamber, OutCheck isSitting)[] checks =
        [
            (Chamber.House, Parliament.IsHouseSittingDay),
            (Chamber.Senate, Parliament.IsSenateSittingDay),
        ];

        foreach (var (chamber, isSitting) in checks)
        {
            var sittings = 0;
            foreach (var year in Parliament.CoveredYears(chamber))
            {
                foreach (var date in Days(year, 1))
                {
                    var expected = date.IsSittingDay(chamber, out var expectedName);
                    if (expected)
                    {
                        sittings++;
                    }

                    await Assert.That(isSitting(date, out var name)).IsEqualTo(expected);
                    await Assert.That(name).IsEqualTo(expectedName);
                }
            }

            await Assert.That(sittings).IsGreaterThan(0);
        }
    }

    [Test]
    public async Task HolidayServiceDelegates()
    {
        var service = new HolidayService(TimeProvider.System);

        await AssertMatchesCore(ServiceChecks(service));

        foreach (var date in Days(2025, 2))
        {
            await Assert.That(service.IsNationalHoliday(date)).IsEqualTo(date.IsNationalHoliday());
            await Assert.That(service.IsNationalHoliday(date, out var nationalName)).IsEqualTo(date.IsNationalHoliday(out var expectedNationalName));
            await Assert.That(nationalName).IsEqualTo(expectedNationalName);
            await Assert.That(service.IsFederalGovernmentShutdown(date)).IsEqualTo(date.IsFederalGovernmentShutdown());

            foreach (var state in Enum.GetValues<State>())
            {
                await Assert.That(service.IsHoliday(date, state)).IsEqualTo(date.IsHoliday(state));
                await Assert.That(service.IsHoliday(date, state, out var name)).IsEqualTo(date.IsHoliday(state, out var expectedName));
                await Assert.That(name).IsEqualTo(expectedName);

                await Assert.That(service.IsSchoolHoliday(date, state)).IsEqualTo(date.IsSchoolHoliday(state));
                await Assert.That(service.IsSchoolHoliday(date, state, out var schoolName)).IsEqualTo(date.IsSchoolHoliday(state, out var expectedSchoolName));
                await Assert.That(schoolName).IsEqualTo(expectedSchoolName);
            }
        }

        await Assert.That(service.ForNational(2025)).IsEquivalentTo(Holidays.ForNational(2025));
        await Assert.That(service.GetFederalGovernmentShutdown(2025)).IsEqualTo(Holidays.GetFederalGovernmentShutdown(2025));

        foreach (var state in Enum.GetValues<State>())
        {
            await Assert.That(service.ForYears(state, 2025, 2)).IsEquivalentTo(Holidays.ForYears(state, 2025, 2));
            await Assert.That(service.GetSchoolTerms(state, 2025)).IsEquivalentTo(SchoolHolidays.GetTerms(state, 2025));
            await Assert.That(service.GetSchoolHolidays(state, 2025)).IsEquivalentTo(SchoolHolidays.GetHolidays(state, 2025));
            await Assert.That(await service.ExportToMarkdown(state, 2025)).IsEqualTo(await Holidays.ExportToMarkdown(state, 2025));

            await using var stateWriter = new StringWriter();
            await service.ExportToMarkdown(stateWriter, state, 2025);
            await Assert.That(stateWriter.ToString()).IsEqualTo(await Holidays.ExportToMarkdown(state, 2025));
        }

        await Assert.That(await service.ExportToMarkdown(2025)).IsEqualTo(await Holidays.ExportToMarkdown(2025));

        await using var writer = new StringWriter();
        await service.ExportToMarkdown(writer, 2025);
        await Assert.That(writer.ToString()).IsEqualTo(await Holidays.ExportToMarkdown(2025));
    }

    [Test]
    public async Task AlwaysHolidayServiceIsAlwaysHoliday()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2025, 3, 3);

        foreach (var check in ServiceChecks(service))
        {
            await Assert.That(check.Is(date)).IsTrue();
            await Assert.That(check.IsWithName(date, out var name)).IsTrue();
            await Assert.That(name).IsEqualTo($"{check.State} Holiday");

            var dates = check.For(2025);
            await Assert.That(dates.Count).IsEqualTo(365);
            await Assert.That(dates.Values.Distinct().Single()).IsEqualTo($"{check.State} Holiday");
        }

        await Assert.That(service.IsNationalHoliday(date)).IsTrue();
        await Assert.That(service.IsNationalHoliday(date, out var nationalName)).IsTrue();
        await Assert.That(nationalName).IsEqualTo("National Holiday");
        await Assert.That(service.ForNational(2025)).IsEquivalentTo(Holidays.ForNational(2025));
        await Assert.That(service.IsFederalGovernmentShutdown(date)).IsTrue();
        await Assert.That(service.GetFederalGovernmentShutdown(2025)).IsEqualTo((Date.MinValue, Date.MaxValue));
        await Assert.That(service.NationalForYears(2025).Count()).IsEqualTo(365);
    }

    [Test]
    public async Task NeverHolidayServiceIsNeverHoliday()
    {
        var service = new NeverHolidayService();
        var date = new Date(2025, 1, 1);

        foreach (var check in ServiceChecks(service))
        {
            await Assert.That(check.Is(date)).IsFalse();
            await Assert.That(check.IsWithName(date, out var name)).IsFalse();
            await Assert.That(name).IsNull();
            await Assert.That(check.For(2025)).IsEmpty();
        }

        foreach (var state in Enum.GetValues<State>())
        {
            await Assert.That(service.IsHoliday(date, state)).IsFalse();
            await Assert.That(service.IsHoliday(date, state, out var name)).IsFalse();
            await Assert.That(name).IsNull();
            await Assert.That(service.ForYears(state, 2025)).IsEmpty();
        }

        await Assert.That(service.IsNationalHoliday(date)).IsFalse();
        await Assert.That(service.IsNationalHoliday(date, out var nationalName)).IsFalse();
        await Assert.That(nationalName).IsNull();
        await Assert.That(service.ForNational(2025)).IsEmpty();
        await Assert.That(service.NationalForYears(2025)).IsEmpty();
        await Assert.That(service.IsFederalGovernmentShutdown(date)).IsFalse();
        await Assert.That(service.GetFederalGovernmentShutdown(2025)).IsEqualTo((Date.MinValue, Date.MinValue));
        await Assert.That(await service.ExportToMarkdown(2025)).IsEmpty();
    }
}
