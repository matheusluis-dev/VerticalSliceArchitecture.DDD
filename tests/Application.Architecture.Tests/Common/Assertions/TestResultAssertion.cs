namespace Application.Architecture.Tests.Common.Assertions;

using System.Text;

internal static class TestResultAssertion
{
    internal static void ShouldBeSuccessful(this TestResult? result, string? message = null)
    {
        result.Should().NotBeNull();

        var sb = new StringBuilder();

        if (message is not null)
            sb.AppendLine().AppendLine(message);

        sb.Append(result.ExplanationMessage());

        result!.IsSuccessful.Should().BeTrue(sb.ToString());
    }

    internal static void ShouldBeEmpty(this IEnumerable<string> enumerable)
    {
        var message = $$"""

            Inconsistences were found in those elements:
            {{enumerable.ToUnorderedStringList('-', 2)}}
            
            """;

        enumerable.Should().BeEmpty(message);
    }

    private static string ExplanationMessage(this TestResult? result)
    {
        if (result is null)
            return string.Empty;

        if (result.IsSuccessful)
            return string.Empty;

        if (result.FailingTypes.Any(f => !string.IsNullOrWhiteSpace(f.Explanation)))
        {
            return $$"""

                Inconsistences were found in those elements:
                {{result.ExplanationsUnorderedListString('-', 2)}}
                
                """;
        }

        return $$"""

            Inconsistences were found in those types:
            {{result.FailingTypesUnorderedListString('-', 2)}}
            
            """;
    }
}
