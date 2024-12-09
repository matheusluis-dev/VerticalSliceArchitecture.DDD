namespace Application.Architecture.Tests.Common.Assertions;

internal static class TestResultAssertion
{
    internal static void ShouldBeSuccessful(this TestResult? result)
    {
        result.Should().NotBeNull();

        result!.IsSuccessful.Should().BeTrue(result.ExplanationMessage());
    }

    private static string ExplanationMessage(this TestResult? result)
    {
        if (result is null)
            return string.Empty;

        if (result.IsSuccessful)
            return string.Empty;

        if (result.FailingTypes.All(f => string.IsNullOrWhiteSpace(f.Explanation)))
            return string.Empty;

        return $$"""

            Inconsistences were found in those elements:
            {{result.ExplanationsUnorderedListString('-', 2)}}
            
            """;
    }
}
