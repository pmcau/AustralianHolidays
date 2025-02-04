public static class ModuleInitializer
{
    //FileExtensions.RemoveTextExtension("ics");
    [ModuleInitializer]
    public static void Init() => VerifierSettings.InitializePlugins();
}