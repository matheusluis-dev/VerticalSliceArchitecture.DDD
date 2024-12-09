namespace Application.Architecture.Tests.Common.CustomRules;

using System;

internal sealed class ShouldHaveSuffixCustomRule(string suffix) : ICustomRule2
{
    private readonly string _suffix = suffix;

    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var endsWithSuffix = type.Name.EndsWith(_suffix, StringComparison.Ordinal);
        var message = endsWithSuffix
            ? $"'{type.Name}' => Has '{_suffix}' suffix"
            : $"'{type.Name}' => Does not have '{_suffix}' suffix";

        return new CustomRuleResult(endsWithSuffix, message);
    }
}
