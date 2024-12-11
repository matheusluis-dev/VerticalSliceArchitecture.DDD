//namespace Application.Architecture.Tests.Common.CustomRules;

//internal sealed class ShouldHaveSuffixCustomRule(string suffix) : ICustomRule2
//{
//    private readonly string _suffix = suffix;

//    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
//    {
//        ArgumentNullException.ThrowIfNull(type);

//        var endsWithSuffix = type.Name.EndsWith(_suffix, StringComparison.Ordinal);
//        var message = endsWithSuffix
//            ? $"Has '{_suffix}' suffix"
//            : $"Does not have '{_suffix}' suffix";

//        return new CustomRuleResult(endsWithSuffix, message);
//    }
//}
