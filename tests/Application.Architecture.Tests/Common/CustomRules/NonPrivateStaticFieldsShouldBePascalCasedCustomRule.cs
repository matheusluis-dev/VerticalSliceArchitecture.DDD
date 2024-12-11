namespace Application.Architecture.Tests.Common.CustomRules;

using System.Linq;
using Mono.Cecil;

public sealed class NonPrivateStaticFieldsShouldBePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var nonCompliantFields = type
            .Fields.Where(f =>
                !f.IsSpecialName
                && !f.IsRuntimeSpecialName
                && !f.HasCustomAttributes
                && !f.IsPrivate
                && f.IsStatic
                && f.Name.IsNotPascalCased()
            )
            .Select(f => f.Name);

        var hasNonCompliantFields = nonCompliantFields.Any();
        var message = hasNonCompliantFields
            ? $$"""
                Has non-private static fields with name not PascalCased:
                {{nonCompliantFields.ToUnorderedStringList('>', 4)}}
                """
            : "Has not non-private static fields with name not PascalCased";

        return new CustomRuleResult(!hasNonCompliantFields, message);
    }
}
