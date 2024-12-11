namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class PrivateFieldsShouldBeCamelCasedWithUnderscorePrefixCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var nonCompliantFields = type
            .Fields.Where(f =>
                !f.IsSpecialName
                && !f.IsRuntimeSpecialName
                && !f.HasCustomAttributes
                && f.IsPrivate
                && f.Name.IsNotUnderscoreCamelCased()
            )
            .Select(f => f.Name);

        var hasNonCompliantFields = nonCompliantFields.Any();
        var message = hasNonCompliantFields
            ? $$"""
                Has fields with name not _camelCased:
                {{nonCompliantFields.ToUnorderedStringList('>', 4)}}
                """
            : "Has not fields with name not _camelCased";

        return new CustomRuleResult(!hasNonCompliantFields, message);
    }
}
