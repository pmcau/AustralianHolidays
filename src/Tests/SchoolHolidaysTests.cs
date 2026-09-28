public class SchoolHolidaysTests
{
    [Test]
    [MethodDataSource(nameof(GetStates))]
    public Task SchoolCalendar(State state)
    {
        var builder = new StringBuilder();
        foreach (var year in SchoolHolidays.CoveredYears(state))
        {
            builder.AppendLine(year.ToString());
            builder.AppendLine("  Terms");
            foreach (var (number, start, end) in SchoolHolidays.GetTerms(state, year))
            {
                builder.AppendLine($"    Term {number}: {Format(start)} - {Format(end)}");
            }

            builder.AppendLine("  Holidays");
            foreach (var (start, end, name) in SchoolHolidays.GetHolidays(state, year))
            {
                builder.AppendLine($"    {name}: {Format(start)} - {Format(end)}");
            }
        }

        return Verify(builder);
    }

    // Guards against transcription errors: within every covered year the four terms and the derived
    // vacation periods must partition the calendar with no gaps or overlaps, and IsSchoolHoliday must
    // agree with that partition.
    [Test]
    [MethodDataSource(nameof(GetStates))]
    public async Task TermsAndHolidaysAreConsistent(State state)
    {
        foreach (var year in SchoolHolidays.CoveredYears(state))
        {
            var terms = SchoolHolidays.GetTerms(state, year);
            await Assert.That(terms.Count).IsEqualTo(4);

            for (var i = 0; i < 4; i++)
            {
                await Assert.That(terms[i].start <= terms[i].end).IsTrue().Because($"{state} {year} Term {i + 1} starts after it ends");
            }

            for (var i = 0; i < 3; i++)
            {
                await Assert.That(terms[i].end < terms[i + 1].start).IsTrue().Because($"{state} {year} no gap between Term {i + 1} and Term {i + 2}");
            }

            var holidays = SchoolHolidays.GetHolidays(state, year);
            var byName = holidays.ToDictionary(_ => _.name, _ => (_.start, _.end));

            await Assert.That(byName["Autumn"]).IsEqualTo((terms[0].end.AddDays(1), terms[1].start.AddDays(-1)));
            await Assert.That(byName["Winter"]).IsEqualTo((terms[1].end.AddDays(1), terms[2].start.AddDays(-1)));
            await Assert.That(byName["Spring"]).IsEqualTo((terms[2].end.AddDays(1), terms[3].start.AddDays(-1)));

            // Summer leads into Term 1 and ends the day before it starts.
            await Assert.That(byName["Summer"].end).IsEqualTo(terms[0].start.AddDays(-1));
            if (SchoolHolidays.CoveredYears(state).Contains(year - 1))
            {
                var previous = SchoolHolidays.GetTerms(state, year - 1);
                await Assert.That(byName["Summer"].start).IsEqualTo(previous[3].end.AddDays(1));
            }

            // Every day from 1 January to the last day of Term 4 is either in a term or in exactly one
            // vacation period, and IsSchoolHoliday reports the inverse of "in a term".
            for (var date = new Date(year, 1, 1); date <= terms[3].end; date = date.AddDays(1))
            {
                var inTerm = terms.Any(_ => date >= _.start && date <= _.end);
                var inVacation = holidays.Any(_ => date >= _.start && date <= _.end);
                await Assert.That(inTerm ^ inVacation).IsTrue().Because($"{state} {date:yyyy-MM-dd} is in both or neither a term and a vacation");
                await Assert.That(date.IsSchoolHoliday(state)).IsEqualTo(!inTerm).Because($"{state} {date:yyyy-MM-dd} IsSchoolHoliday disagrees with term membership");
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(GetStates))]
    public async Task TermBoundariesAreNotHolidays(State state)
    {
        foreach (var year in SchoolHolidays.CoveredYears(state))
        {
            foreach (var (number, start, end) in SchoolHolidays.GetTerms(state, year))
            {
                await Assert.That(start.IsSchoolHoliday(state)).IsFalse().Because($"{state} {year} Term {number} first day reported as a holiday");
                await Assert.That(end.IsSchoolHoliday(state)).IsFalse().Because($"{state} {year} Term {number} last day reported as a holiday");
                await Assert.That(start.AddDays(-1).IsSchoolHoliday(state)).IsTrue().Because($"{state} {year} day before Term {number} not a holiday");
                await Assert.That(end.AddDays(1).IsSchoolHoliday(state)).IsTrue().Because($"{state} {year} day after Term {number} not a holiday");
            }
        }
    }

    [Test]
    public async Task UncoveredYearThrows()
    {
        await Assert.That(() => SchoolHolidays.GetTerms(State.NSW, 1980)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => SchoolHolidays.GetHolidays(State.NSW, 1980)).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task UncoveredYearIsNotAHoliday() =>
        await Assert.That(new Date(1980, 7, 1).IsSchoolHoliday(State.NSW)).IsFalse();

    [Test]
    public async Task IsSchoolHolidayUsage()
    {
        #region IsSchoolHoliday

        var date = new Date(2026, 4, 10);

        await Assert.That(date.IsSchoolHoliday(State.NSW)).IsTrue();

        #endregion
    }

    [Test]
    public async Task IsSchoolHolidayNamedUsage()
    {
        #region IsSchoolHolidayNamed

        var date = new Date(2026, 4, 10);

        await Assert.That(date.IsSchoolHoliday(State.NSW, out var name)).IsTrue();

        await Assert.That(name).IsEqualTo("Autumn");

        #endregion
    }

    [Test]
    public async Task IsSchoolHolidayForStateUsage()
    {
        #region IsSchoolHolidayForState

        var date = new Date(2026, 4, 10);

        await Assert.That(date.IsNswSchoolHoliday()).IsTrue();

        #endregion
    }

    [Test]
    public async Task GetTermsUsage()
    {
        #region GetSchoolTerms

        var terms = SchoolHolidays.GetTerms(State.NSW, 2026);
        foreach (var (number, start, end) in terms)
        {
            Console.WriteLine($"Term {number}: {start} - {end}");
        }

        #endregion

        await Assert.That(terms.Count).IsEqualTo(4);
    }

    [Test]
    public async Task GetHolidaysUsage()
    {
        #region GetSchoolHolidays

        var holidays = SchoolHolidays.GetHolidays(State.NSW, 2026);
        foreach (var (start, end, name) in holidays)
        {
            Console.WriteLine($"{name}: {start} - {end}");
        }

        #endregion

        await Assert.That(holidays.Count).IsEqualTo(4);
    }

    static string Format(Date date) =>
        date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture);

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();
}
