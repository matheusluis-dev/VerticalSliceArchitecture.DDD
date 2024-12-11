namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class TypeShouldHaveNamePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var isPascalCased = type.Name.IsPascalCased();
        var message = isPascalCased ? "Is PascalCased" : "Is not PascalCased";

        return new CustomRuleResult(isPascalCased, message);
    }
}
