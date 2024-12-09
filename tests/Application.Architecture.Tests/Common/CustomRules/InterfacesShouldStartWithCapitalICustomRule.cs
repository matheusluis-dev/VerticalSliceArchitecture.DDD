namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class InterfacesShouldStartWithCapitalICustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.Name[0].Equals('I'))
            return new CustomRuleResult(false, $"'{type.Name}' => Does not starts with capital I");

        if (type.Name.Length == 1)
            return new CustomRuleResult(false, $"'{type.Name}' => Should have more than 1 char");

        if (char.IsLower(type.Name[1]))
        {
            return new CustomRuleResult(
                false,
                $"'{type.Name}' => Second char should be uppercased"
            );
        }

        return new CustomRuleResult(type.Name.IsPascalCased(), $"'{type.Name}' => Is PascalCased");
    }
}
