namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

internal sealed class NonAbstractNonStaticNonPartialClassesShouldBeSealedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var partialClasses = PartialClassesHelper.Types;

        if (type.IsStatic())
            return new CustomRuleResult(true, $"'Is Static.");

        if (type.IsAbstract)
            return new CustomRuleResult(true, $"'Is Abstract.");

        if (partialClasses.Contains(type.Name, StringComparer.Ordinal))
            return new CustomRuleResult(true, $"'Is Partial.");

        if (type.IsSealed)
            return new CustomRuleResult(true, $"'Is Sealed.");

        return new CustomRuleResult(
            false,
            $"'Is not Static, is not Abstract, is not Partial and is not Sealed"
        );
    }
}
