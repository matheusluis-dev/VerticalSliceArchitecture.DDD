namespace Application.Architecture.Tests.Common.Extensions;

using System.Linq;

internal static class TestResultExtensions
{
    public static string ExplanationsUnorderedListString(
        this TestResult? result,
        char marker,
        int? tabWidth = null
    )
    {
        if (result is null)
            return string.Empty;

        return result
            .FailingTypes.Where(f => !string.IsNullOrWhiteSpace(f.Explanation))
            .Select(f => f.Explanation)
            .ToUnorderedStringList(marker, tabWidth);
    }
}
