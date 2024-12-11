namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class PropertiesShouldHaveNamePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var nonPascalCasedProperties = type
            .Properties.Where(p =>
                !p.IsSpecialName && !p.IsRuntimeSpecialName && p.Name.IsNotPascalCased()
            )
            .Select(p => p.Name);

        var hasNonPascalCasedProperties = nonPascalCasedProperties.Any();
        var message = hasNonPascalCasedProperties
            ? $$"""
                Has properties with name not PascalCased:
                {{nonPascalCasedProperties.ToUnorderedStringList('>', 4)}}
                """
            : $"Has not properties with name not PascalCased";

        return new CustomRuleResult(!hasNonPascalCasedProperties, message);
    }
}
