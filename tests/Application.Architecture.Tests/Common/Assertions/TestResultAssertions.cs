namespace Application.Architecture.Tests.Common.Assertions;

using FluentAssertions.Execution;
using FluentAssertions.Primitives;

public sealed class TestResultAssertions
    : ReferenceTypeAssertions<TestResult?, TestResultAssertions>
{
    public TestResultAssertions(TestResult? testResult)
        : base(testResult) { }

    protected override string Identifier => "TestResult";

    [CustomAssertion]
    public AndConstraint<TestResultAssertions> BeSuccessful(string because = "")
    {
        because = string.IsNullOrWhiteSpace(because)
            ? "Expected TestResult to be Successful"
            : because;

        Execute
            .Assertion.BecauseOf(because)
            .ForCondition(Subject is not null)
            .FailWith("TestResult can not be null")
            .Then.ForCondition(Subject!.IsSuccessful)
            .FailWith($"{because}, but {ExplanationMessage(Subject)}");

        return new AndConstraint<TestResultAssertions>(this);
    }

    private static string ExplanationMessage(TestResult? result)
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
