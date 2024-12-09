namespace Application.Architecture.Tests.Common.CustomRules;

public sealed class AsyncMethodsShouldHaveAsyncSuffixCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule(TypeDefinition type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var nonAsyncSuffixMethods = new List<string>();
        foreach (var method in type.ToType().GetAsyncMethods())
        {
            if (!method.Name.EndsWith("Async", StringComparison.Ordinal))
            {
                nonAsyncSuffixMethods.Add(method.Name);
            }
        }

        var hasNonAsyncSuffixMethods = nonAsyncSuffixMethods.Count > 0;
        var message = hasNonAsyncSuffixMethods
            ? $$"""
                '{{type.Name}}' => has async methods without 'Async' suffix:
                {{nonAsyncSuffixMethods.ToUnorderedStringList('>', 4)}}
                """
            : $"'{type.Name} has not async methods without 'Async' suffix";

        return new CustomRuleResult(hasNonAsyncSuffixMethods, message);
    }
}
