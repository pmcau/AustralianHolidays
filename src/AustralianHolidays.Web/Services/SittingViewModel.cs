public record SittingViewModel(
    Chamber Chamber,
    int Year,
    string Name,
    Date Start,
    Date End,
    IReadOnlyList<Chamber> SittingChambers,
    HolidayTimeCategory TimeCategory)
{
    // Sitting periods are always runs of consecutive weekdays, so there is no weekday/total split to
    // draw the way there is for school holidays.
    public int Days => End.DayNumber - Start.DayNumber + 1;
}
