[TestFixture]
public class Tests
{
    [TestCaseSource(nameof(GetStates))]
    public Task IsPublicHolidayWithName(State state)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsPublicHoliday(state, out var name))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)} {name}");
            }
        }

        return Verify(builder);
    }

    [TestCaseSource(nameof(GetStates))]
    public Task IsPublicHoliday(State state)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsPublicHoliday(state))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}");
            }
        }

        return Verify(builder);
    }

    public static IEnumerable<State> GetStates() =>
        Enum.GetValues<State>();
}