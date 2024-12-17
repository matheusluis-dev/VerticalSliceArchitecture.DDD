namespace Application.Architecture.Tests.Common.Assertions;

public static class TestResultAssertionsExtensions
{
    public static TestResultAssertions Should(this TestResult? testResult)
    {
        return new TestResultAssertions(testResult);
    }
}
