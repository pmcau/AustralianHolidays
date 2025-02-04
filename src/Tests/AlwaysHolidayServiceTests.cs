[TestFixture]
public class AlwaysHolidayServiceTests
{
    #region AlwaysHolidayServiceUsage

    [Test]
    public void AlwaysHolidayServiceUsage()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForYears(2023, 1).ToList();

        AreEqual(8 * 365, result.Count); // 8 states * 365 days
        IsTrue(result.All(item => item.name == "Holiday"));
    }

    #endregion

    [Test]
    public void ForYears_ShouldReturnAllDaysForAllStates()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForYears(2023, 1).ToList();

        AreEqual(8 * 365, result.Count); // 8 states * 365 days
        IsTrue(result.All(item => item.name == "Holiday"));
    }

    [Test]
    public void ForYears_WithState_ShouldReturnAllDaysForState()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForYears(State.NSW, 2023, 1).ToList();

        AreEqual(365, result.Count);
        IsTrue(result.All(item => item.name == "Holiday"));
    }

    [Test]
    public void NationalForYears_ShouldReturnAllDays()
    {
        var service = new AlwaysHolidayService();
        var result = service.NationalForYears(2023, 1).ToList();

        AreEqual(365, result.Count);
        IsTrue(result.All(item => item.name == "Holiday"));
    }

    [Test]
    public void IsHoliday_ShouldReturnTrue()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsHoliday(date, State.NSW);

        IsTrue(result);
    }

    [Test]
    public void IsHoliday_WithName_ShouldReturnTrueAndName()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsHoliday(date, State.NSW, out var name);

        IsTrue(result);
        AreEqual("Holiday", name);
    }

    [Test]
    public void IsActHoliday_ShouldReturnTrue()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsActHoliday(date);

        IsTrue(result);
    }

    [Test]
    public void IsActHoliday_WithName_ShouldReturnTrueAndName()
    {
        var service = new AlwaysHolidayService();
        var date = new Date(2023, 1, 1);
        var result = service.IsActHoliday(date, out var name);

        IsTrue(result);
        AreEqual("ACT Holiday", name);
    }

    [Test]
    public void ForAct_ShouldReturnAllDaysWithNames()
    {
        var service = new AlwaysHolidayService();
        var result = service.ForAct(2023);

        AreEqual(365, result.Count);
        IsTrue(result.Values.All(name => name == "ACT Holiday"));
    }
}