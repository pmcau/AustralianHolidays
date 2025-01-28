[TestFixture]
public class Tests
{
    [Test]
    public Task Write10YearsOfHolidays()
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsPublicHoliday(out var name))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}   {name}");
            }
        }

        return Verify(builder);
    }
}