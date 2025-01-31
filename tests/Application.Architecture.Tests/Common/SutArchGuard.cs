namespace Application.Architecture.Tests.Common;

using System;
using ArchGuard;

internal static class SutArchGuard
{
    private static readonly Lazy<ITypeFilterEntryPoint> _types = new(
        ArchGuard.Types.InSolution(
            new SolutionSearchParameters
            {
                SolutionPath =
                    "C:/Users/matheus.oliveira/source/repos/GitHub/VerticalSliceArchitecture.DDD/VerticalSliceArchitecture.DDD.sln",
                Preprocessor = "net9_0",
                ProjectName = "Application",
            }
        )
    );

    internal static ITypeFilterEntryPoint Types => _types.Value;
}
