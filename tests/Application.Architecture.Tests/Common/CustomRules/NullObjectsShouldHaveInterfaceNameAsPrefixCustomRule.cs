namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class NullObjectsShouldHaveInterfaceNameAsPrefixCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        var interfaceImplemented = type.Interfaces.FirstOrDefault()?.InterfaceType;
        var interfaceName = interfaceImplemented?.Name;

        // Remove "I" prefix from interface
        var expectedTypeNameStart = interfaceName?[1..];

        var isCompliant =
            expectedTypeNameStart is not null
            && type.Name.StartsWith(expectedTypeNameStart, StringComparison.Ordinal);

        var message =
            $"Excepted type name to match interface name => [{interfaceName}], found name => [{type.Name}]";

        return new CustomRuleResult(isCompliant, message);
    }
}
