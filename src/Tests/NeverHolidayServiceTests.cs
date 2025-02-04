[TestFixture]
public class NeverHolidayServiceTests
{
    #region NeverHolidayServiceUsage

    [Test]
    public void NeverHolidayServiceUsage()
    {
        var service = new NeverHolidayService();
        var result = service.ForYears(2023, 1).ToList();

        IsEmpty(result);

        var date = new Date(2020, 1, 2);
        IsFalse(service.IsNswHoliday(date));
    }

    #endregion
}