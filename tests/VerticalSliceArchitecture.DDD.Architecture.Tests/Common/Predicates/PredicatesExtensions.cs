namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Predicates;

internal static class PredicatesExtensions
{
    internal static string[] GetModuleTypes(this PredicateList predicates)
    {
        return predicates
            .GetTypes()
            .Select(type => type.FullName)
            .Distinct()
            .ToArray()!;
    }
}
