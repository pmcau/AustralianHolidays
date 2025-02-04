using EmptyFiles;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        //FileExtensions.RemoveTextExtension("ics");
        VerifierSettings.InitializePlugins();
    }
}