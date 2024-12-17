namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class AsyncMethodsShouldReturnTaskCompletedTaskInsteadOfEmptyReturnCustomRule
    : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        // TODO
        return new CustomRuleResult(true);
    }
}
