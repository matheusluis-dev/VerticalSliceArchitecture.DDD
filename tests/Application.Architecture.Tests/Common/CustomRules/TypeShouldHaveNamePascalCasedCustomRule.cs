namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class TypeShouldHaveNamePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return new CustomRuleResult(
            type.Name.IsPascalCased(),
            $"'{type.Name}' => Is not PascalCased"
        );
    }
}
