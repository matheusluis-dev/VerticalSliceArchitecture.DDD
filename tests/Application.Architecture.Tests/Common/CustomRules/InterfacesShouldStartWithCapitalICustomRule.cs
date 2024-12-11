namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class InterfacesShouldStartWithCapitalICustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        if (!type.Name[0].Equals('I'))
            return new CustomRuleResult(false, $"Does not starts with capital I");

        if (type.Name.Length == 1)
            return new CustomRuleResult(false, $"Should have more than 1 char");

        if (char.IsLower(type.Name[1]))
        {
            return new CustomRuleResult(false, $"Second char should be uppercased");
        }

        return new CustomRuleResult(type.Name.IsPascalCased(), $"Is PascalCased");
    }
}
