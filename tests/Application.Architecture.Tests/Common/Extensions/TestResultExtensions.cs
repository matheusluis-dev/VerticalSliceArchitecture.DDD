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
            .Select(f => $"'{f.FullName}' => {f.Explanation}")
            .ToUnorderedStringList(marker, tabWidth);
    }

    public static string FailingTypesUnorderedListString(
        this TestResult? result,
        char marker,
        int? tabWidth = null
    )
    {
        if (result is null)
            return string.Empty;

        return result.FailingTypes.Select(f => f.FullName).ToUnorderedStringList(marker, tabWidth);
    }
}
