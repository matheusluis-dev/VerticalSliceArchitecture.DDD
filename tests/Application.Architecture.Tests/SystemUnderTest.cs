namespace Application.Architecture.Tests;

using System;
using ArchGuard;

internal static class SystemUnderTest
{
    private static readonly Lazy<ITypeFilterEntryPoint> _types = new(
        ArchGuard.Types.InSolution(
            new SolutionSearchParameters
            {
                SolutionPath = "VerticalSliceArchitecture.DDD/VerticalSliceArchitecture.DDD.sln",
                Preprocessor = "net9_0",
                ProjectName = "Application",
            }
        )
    );

    internal static ITypeFilterEntryPoint Types => _types.Value;
}
