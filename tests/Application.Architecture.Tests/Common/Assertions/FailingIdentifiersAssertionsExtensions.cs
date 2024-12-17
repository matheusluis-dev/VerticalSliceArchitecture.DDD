namespace Application.Architecture.Tests.Common.Assertions;

public static class FailingIdentifiersAssertionsExtensions
{
    public static FailingIdentifiersAssertions Should(this FailingIdentifiers failingIdentifiers)
    {
        return new FailingIdentifiersAssertions(failingIdentifiers);
    }
}
