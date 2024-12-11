namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class MethodsShouldHaveNamePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var nonPascalCasedMethods = type.ToType()
            .GetUserDefinedMethods()
            .Where(m => m.Name.IsNotPascalCased())
            .Select(m => m.Name);

        var hasNonPascalCasedMethods = nonPascalCasedMethods.Any();
        var message = hasNonPascalCasedMethods
            ? $$"""
                Has methods with name not PascalCased:
                {{nonPascalCasedMethods.ToUnorderedStringList('>', 4)}}
                """
            : $"Has not methods with name not PascalCased";

        return new CustomRuleResult(!hasNonPascalCasedMethods, message);
    }
}
