[TestFixture]
public class Tests
{
    [TestCaseSource(nameof(GetArea))]
    public Task IsPublicHolidayWithName(Area area)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsPublicHoliday(area, out var name))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)} {name}");
            }
        }

        return Verify(builder);
    }

    [TestCaseSource(nameof(GetArea))]
    public Task IsPublicHoliday(Area area)
    {
        var builder = new StringBuilder();
        foreach (var date in DateBuilder.Range())
        {
            if (date.IsPublicHoliday(area))
            {
                builder.AppendLine($"{date.ToString("yyyy MMM dd ddd", CultureInfo.InvariantCulture)}");
            }
        }

        return Verify(builder);
    }

    public static IEnumerable<Area> GetArea() =>
        Enum.GetValues<Area>();
}