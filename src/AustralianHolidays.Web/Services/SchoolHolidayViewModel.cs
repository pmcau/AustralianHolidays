public record SchoolHolidayViewModel(
    State State,
    int Year,
    string Name,
    Date Start,
    Date End,
    HolidayTimeCategory TimeCategory)
{
    public int Days => End.DayNumber - Start.DayNumber + 1;

    public string Duration
    {
        get
        {
            var weeks = Math.Round(Days / 7d * 2, MidpointRounding.AwayFromZero) / 2;
            var unit = weeks == 1 ? "wk" : "wks";
            return $"{weeks:0.#} {unit}";
        }
    }
}
