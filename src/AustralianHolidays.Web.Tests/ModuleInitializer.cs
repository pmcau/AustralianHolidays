static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        VerifyPlaywright.Initialize(installPlaywright: true);
        VerifyImageMagick.Initialize();
        VerifyImageMagick.RegisterComparers(.01);
        VerifierSettings.InitializePlugins();
    }
}
