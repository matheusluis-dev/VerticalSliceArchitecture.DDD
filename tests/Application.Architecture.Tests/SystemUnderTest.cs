namespace Application.Architecture.Tests;

using ArchGuard;

internal static class SystemUnderTest
{
    internal static ITypeFilterEntryPoint Types =>
        ArchGuard.Types.InSolution(
            "VerticalSliceArchitecture.DDD/VerticalSliceArchitecture.DDD.sln",
            "Application",
            "net9_0"
        );
}
