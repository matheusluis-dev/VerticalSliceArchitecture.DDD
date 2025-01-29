namespace Application.Architecture.Tests.Common;

using System;
using ArchGuard.Solution;
using ArchGuard.Type.Filters.EntryPoint;

internal static class SutArch
{
    private static readonly Lazy<ITypeDefinitionFilterEntryPoint> _types = new(
        () =>
            ArchGuard.Type.Types.InSolution(
                new SolutionSearchParameters
                {
                    SlnPath =
                        "C:/Users/matheus.oliveira/source/repos/GitHub/VerticalSliceArchitecture.DDD/VerticalSliceArchitecture.DDD.sln",
                    Preprocessor = "net9_0",
                    ProjectName = "Application",
                }
            )
    );

    internal static ITypeDefinitionFilterEntryPoint Types => _types.Value;
}
