namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class NullObjectsMethodsThatDoesNotHaveReturnTypeShouldBeEmptyCustomRule
    : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        var nonCompliantMethods = type.GetUserDefinedMethods()
            .Where(m => m.ReturnType == typeof(void) && m.IsMethodBodyNotEmpty())
            .Select(m => m.Name);

        var hasNonCompliantMethods = nonCompliantMethods.Any();
        var message = hasNonCompliantMethods
            ? $$"""
                Has non-empty void methods:
                {{nonCompliantMethods.ToUnorderedStringList('>', 4)}}
                """
            : "Has not non-empty void methods";

        return new CustomRuleResult(!hasNonCompliantMethods, message);
    }
}
