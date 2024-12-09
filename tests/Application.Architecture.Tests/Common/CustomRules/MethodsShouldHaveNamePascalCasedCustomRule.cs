namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class MethodsShouldHaveNamePascalCasedCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var nonPascalCasedMethods = new List<string>();
        foreach (var method in type.ToType().GetUserDefinedMethods())
        {
            if (method.Name.IsNotPascalCased())
            {
                nonPascalCasedMethods.Add(method.Name);
            }
        }

        var hasNonPascalCasedMethods = nonPascalCasedMethods.Count == 0;
        var message = hasNonPascalCasedMethods
            ? $$"""
                '{{type.Name}}' => has methods with name not PascalCased:
                {{nonPascalCasedMethods.ToUnorderedStringList('>', 4)}}
                """
            : $"'{type.Name}' => has not methods with name not PascalCased";

        return new CustomRuleResult(hasNonPascalCasedMethods, message);
    }
}
