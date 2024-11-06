namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Rules;

using Mono.Cecil;

using static PartialClassFinder;

public sealed class AreNotPartialCustomRule : ICustomRule
{
    public bool MeetsRule(TypeDefinition type)
    {
        var partialClasses = GetPartialClassNames();

        var isPartial = partialClasses.Any(p => p.Equals(type.Name));
        return !isPartial;
    }
}
