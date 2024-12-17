namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class NullObjectsShouldImplementOneInterfaceCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        if (!type.HasInterfaces)
            return new CustomRuleResult(false, "NullObject does not implement any interface");

        if (type.Interfaces.Count > 1)
        {
            var message = $$"""
                NullObject implements more than 1 interface:
                {{type.Interfaces.Select(i => i.InterfaceType.FullName).ToUnorderedStringList(
                    '>',
                    4
                )}}
                """;

            return new CustomRuleResult(false, message);
        }

        return new CustomRuleResult(true, "NullObject implements 1 interface");
    }
}
