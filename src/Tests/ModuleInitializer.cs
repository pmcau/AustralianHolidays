public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifierSettings.InitializePlugins();
        VerifierSettings.ScrubLinesWithReplace(line =>
            line.StartsWith("DTSTAMP:") ? "DTSTAMP:Scrubbed" : line);
    }
}