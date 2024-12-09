namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

internal sealed class NonAbstractNonStaticNonPartialClassesShouldBeSealedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        var partialClasses = PartialClassesHelper.Types;

        if (type.IsStatic())
            return new CustomRuleResult(true, $"'{type.Name}' => Is Static.");

        if (type.IsAbstract)
            return new CustomRuleResult(true, $"'{type.Name}' => Is Abstract.");

        if (partialClasses.Contains(type.Name, StringComparer.Ordinal))
            return new CustomRuleResult(true, $"'{type.Name}' => Is Partial.");

        if (type.IsSealed)
            return new CustomRuleResult(true, $"'{type.Name}' => Is Sealed.");

        return new CustomRuleResult(
            false,
            $"'{type.Name}' => Is not Static, is not Abstract, is not Partial and is not Sealed"
        );
    }
}
