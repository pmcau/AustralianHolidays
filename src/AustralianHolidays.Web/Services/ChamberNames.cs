public static class ChamberNames
{
    public static string Short(Chamber chamber) =>
        chamber switch
        {
            Chamber.House => "House",
            Chamber.Senate => "Senate",
            _ => chamber.ToString()
        };

    public static string Full(Chamber chamber) =>
        chamber switch
        {
            Chamber.House => "House of Representatives",
            Chamber.Senate => "Senate",
            _ => chamber.ToString()
        };
}
