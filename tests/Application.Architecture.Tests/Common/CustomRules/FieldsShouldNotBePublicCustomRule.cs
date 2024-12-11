namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class FieldsShouldNotBePublicCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var nonCompliantFields = type
            .Fields.Where(f =>
                !f.IsSpecialName
                && !f.IsRuntimeSpecialName
                && !f.HasCustomAttributes
                && !f.IsStatic
                && !f.IsPrivate
                && !f.IsInitOnly
            )
            .Select(f => f.Name);

        var hasNonCompliantFields = nonCompliantFields.Any();
        var message = hasNonCompliantFields
            ? $$"""
                Has non-private fields:
                {{nonCompliantFields.ToUnorderedStringList('>', 4)}}
                """
            : $"Has not non-private fields";

        return new CustomRuleResult(!hasNonCompliantFields, message);
    }
}
