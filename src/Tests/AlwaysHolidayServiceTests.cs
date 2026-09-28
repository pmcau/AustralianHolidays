public class AlwaysHolidayServiceTests
{
    #region AlwaysHolidayServiceUsage

    [Test]
    public async Task AlwaysHolidayServiceUsage()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForYears(2023, 1).ToList();

        await Assert.That(result.Count).IsEqualTo(8 * 365); // 8 states * 365 days
        await Assert.That(result.All(item => item.name == "Holiday")).IsTrue();
    }

    #endregion

    [Test]
    public async Task ForYears_ShouldReturnAllDaysForAllStates()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForYears(2023, 1).ToList();

        await Assert.That(result.Count).IsEqualTo(8 * 365); // 8 states * 365 days
        await Assert.That(result.All(item => item.name == "Holiday")).IsTrue();
    }

    [Test]
    public async Task ForYears_WithState_ShouldReturnAllDaysForState()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForYears(State.NSW, 2023, 1).ToList();

        await Assert.That(result.Count).IsEqualTo(365);
        await Assert.That(result.All(item => item.name == "Holiday")).IsTrue();
    }

    [Test]
    public async Task NationalForYears_ShouldReturnAllDays()
    {
        var service = new AlwaysHolidayService();
        var result = service.NationalForYears(2023, 1).ToList();

        await Assert.That(result.Count).IsEqualTo(365);
        await Assert.That(result.All(item => item.name == "Holiday")).IsTrue();
    }

    [Test]
    public async Task IsHoliday_ShouldReturnTrue()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsHoliday(date, State.NSW);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsHoliday_WithName_ShouldReturnTrueAndName()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsHoliday(date, State.NSW, out var name);

        await Assert.That(result).IsTrue();
        await Assert.That(name).IsEqualTo("Holiday");
    }

    [Test]
    public async Task IsActHoliday_ShouldReturnTrue()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsActHoliday(date);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task IsActHoliday_WithName_ShouldReturnTrueAndName()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsActHoliday(date, out var name);

        await Assert.That(result).IsTrue();
        await Assert.That(name).IsEqualTo("ACT Holiday");
    }

    [Test]
    public async Task ForAct_ShouldReturnAllDaysWithNames()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForAct(2023);

        await Assert.That(result.Count).IsEqualTo(365);
        await Assert.That(result.Values.All(name => name == "ACT Holiday")).IsTrue();
    }
}