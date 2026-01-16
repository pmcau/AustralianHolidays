static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        VerifyPlaywright.Initialize(installPlaywright: true);
        VerifierSettings.InitializePlugins();
    }
}
