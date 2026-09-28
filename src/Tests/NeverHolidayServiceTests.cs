public class NeverHolidayServiceTests
{
    #region NeverHolidayServiceUsage

    [Test]
    public async Task NeverHolidayServiceUsage()
    {
        var service = new NeverHolidayService();
        var result = service.ForYears(2023, 1).ToList();

        await Assert.That(result).IsEmpty();

        var date = new Date(2020, 1, 2);
        await Assert.That(service.IsNswHoliday(date)).IsFalse();
    }

    #endregion
}