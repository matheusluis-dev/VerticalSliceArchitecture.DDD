namespace Application.Architecture.Tests.Common.Assertions;

using FluentAssertions.Execution;
using FluentAssertions.Primitives;

public sealed class FailingIdentifiersAssertions
    : ReferenceTypeAssertions<FailingIdentifiers, FailingIdentifiersAssertions>
{
    public FailingIdentifiersAssertions(FailingIdentifiers failingIdentifiers)
        : base(failingIdentifiers) { }

    protected override string Identifier => "FailingIdentifiers";

    [CustomAssertion]
    public AndConstraint<FailingIdentifiersAssertions> NotContainFailures(string because = "")
    {
        Execute
            .Assertion.BecauseOf(because)
            .ForCondition(Subject is not null)
            .FailWith("FailingIdentifiers can not be null")
            .Then.ForCondition(Subject!.Count == 0)
            .FailWith($"Expected FailingIdentifiers to be Empty, but {Subject.GetErrorMessage()}");

        return new AndConstraint<FailingIdentifiersAssertions>(this);
    }
}
