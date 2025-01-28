[TestFixture]
public class AnzacDayTests
{
    [Test]
    public Task IsAnzacDay()
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsAnzacDay())
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}");
            }
        }

        return Verify(builder);
    }

    [TestCaseSource(nameof(GetStates))]
    public Task IsAnzacDayHoliday(State state)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsAnzacDayHoliday(state))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}");
            }
        }

        return Verify(builder);
    }

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();

    [Test]
    public void GetEasterFriday()
    {
        for (var i = 2025; i <= 2044; i++)
        {
            var easterFriday = EasterCalculator.GetEasterFriday(i);
            AreEqual(DateBuilder.EasterFridays[i - 2025], easterFriday);
        }
    }

    [Test]
    public Task GetEaster() =>
        VerifyTuple(() => EasterCalculator.ForYear(2020))
            .DontScrubDateTimes();

}