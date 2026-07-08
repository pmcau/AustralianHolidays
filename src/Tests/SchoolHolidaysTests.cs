[TestFixture]
public class SchoolHolidaysTests
{
    [TestCaseSource(nameof(GetStates))]
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
    [TestCaseSource(nameof(GetStates))]
    public void TermsAndHolidaysAreConsistent(State state)
    {
        foreach (var year in SchoolHolidays.CoveredYears(state))
        {
            var terms = SchoolHolidays.GetTerms(state, year);
            AreEqual(4, terms.Count);

            for (var i = 0; i < 4; i++)
            {
                IsTrue(terms[i].start <= terms[i].end, $"{state} {year} Term {i + 1} starts after it ends");
            }

            for (var i = 0; i < 3; i++)
            {
                IsTrue(terms[i].end < terms[i + 1].start, $"{state} {year} no gap between Term {i + 1} and Term {i + 2}");
            }

            var holidays = SchoolHolidays.GetHolidays(state, year);
            var byName = holidays.ToDictionary(_ => _.name, _ => (_.start, _.end));

            AreEqual((terms[0].end.AddDays(1), terms[1].start.AddDays(-1)), byName["Autumn"]);
            AreEqual((terms[1].end.AddDays(1), terms[2].start.AddDays(-1)), byName["Winter"]);
            AreEqual((terms[2].end.AddDays(1), terms[3].start.AddDays(-1)), byName["Spring"]);

            // Summer leads into Term 1 and ends the day before it starts.
            AreEqual(terms[0].start.AddDays(-1), byName["Summer"].end);
            if (SchoolHolidays.CoveredYears(state).Contains(year - 1))
            {
                var previous = SchoolHolidays.GetTerms(state, year - 1);
                AreEqual(previous[3].end.AddDays(1), byName["Summer"].start);
            }

            // Every day from 1 January to the last day of Term 4 is either in a term or in exactly one
            // vacation period, and IsSchoolHoliday reports the inverse of "in a term".
            for (var date = new Date(year, 1, 1); date <= terms[3].end; date = date.AddDays(1))
            {
                var inTerm = terms.Any(_ => date >= _.start && date <= _.end);
                var inVacation = holidays.Any(_ => date >= _.start && date <= _.end);
                IsTrue(inTerm ^ inVacation, $"{state} {date:yyyy-MM-dd} is in both or neither a term and a vacation");
                AreEqual(!inTerm, date.IsSchoolHoliday(state), $"{state} {date:yyyy-MM-dd} IsSchoolHoliday disagrees with term membership");
            }
        }
    }

    [TestCaseSource(nameof(GetStates))]
    public void TermBoundariesAreNotHolidays(State state)
    {
        foreach (var year in SchoolHolidays.CoveredYears(state))
        {
            foreach (var (number, start, end) in SchoolHolidays.GetTerms(state, year))
            {
                IsFalse(start.IsSchoolHoliday(state), $"{state} {year} Term {number} first day reported as a holiday");
                IsFalse(end.IsSchoolHoliday(state), $"{state} {year} Term {number} last day reported as a holiday");
                IsTrue(start.AddDays(-1).IsSchoolHoliday(state), $"{state} {year} day before Term {number} not a holiday");
                IsTrue(end.AddDays(1).IsSchoolHoliday(state), $"{state} {year} day after Term {number} not a holiday");
            }
        }
    }

    [Test]
    public void UncoveredYearThrows()
    {
        Throws<ArgumentOutOfRangeException>(() => SchoolHolidays.GetTerms(State.NSW, 1980));
        Throws<ArgumentOutOfRangeException>(() => SchoolHolidays.GetHolidays(State.NSW, 1980));
    }

    [Test]
    public void UncoveredYearIsNotAHoliday() =>
        IsFalse(new Date(1980, 7, 1).IsSchoolHoliday(State.NSW));

    [Test]
    public void IsSchoolHolidayUsage()
    {
        #region IsSchoolHoliday

        var date = new Date(2026, 4, 10);

        IsTrue(date.IsSchoolHoliday(State.NSW));

        #endregion
    }

    [Test]
    public void IsSchoolHolidayNamedUsage()
    {
        #region IsSchoolHolidayNamed

        var date = new Date(2026, 4, 10);

        IsTrue(date.IsSchoolHoliday(State.NSW, out var name));

        AreEqual("Autumn", name);

        #endregion
    }

    [Test]
    public void IsSchoolHolidayForStateUsage()
    {
        #region IsSchoolHolidayForState

        var date = new Date(2026, 4, 10);

        IsTrue(date.IsNswSchoolHoliday());

        #endregion
    }

    [Test]
    public void GetTermsUsage()
    {
        #region GetSchoolTerms

        var terms = SchoolHolidays.GetTerms(State.NSW, 2026);
        foreach (var (number, start, end) in terms)
        {
            Console.WriteLine($"Term {number}: {start} - {end}");
        }

        #endregion

        AreEqual(4, terms.Count);
    }

    [Test]
    public void GetHolidaysUsage()
    {
        #region GetSchoolHolidays

        var holidays = SchoolHolidays.GetHolidays(State.NSW, 2026);
        foreach (var (start, end, name) in holidays)
        {
            Console.WriteLine($"{name}: {start} - {end}");
        }

        #endregion

        AreEqual(4, holidays.Count);
    }

    static string Format(Date date) =>
        date.ToString("ddd dd MMM yyyy", CultureInfo.InvariantCulture);

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();
}
