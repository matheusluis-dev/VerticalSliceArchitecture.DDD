namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class NamespaceShouldHaveNamePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var namespaces = type.GetNamespaceSplited();

        var isPascalCased = namespaces.All(n => string.IsNullOrWhiteSpace(n) || n.IsPascalCased());
        var message = isPascalCased ? "Namespace is PascalCased" : "Namespace Is not PascalCased";

        return new CustomRuleResult(isPascalCased, message);
    }
}
