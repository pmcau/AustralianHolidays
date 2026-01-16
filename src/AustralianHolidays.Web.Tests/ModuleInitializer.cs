using System.Runtime.CompilerServices;
using VerifyTests.Bunit;

static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() =>
        VerifyBunit.Initialize();
}
