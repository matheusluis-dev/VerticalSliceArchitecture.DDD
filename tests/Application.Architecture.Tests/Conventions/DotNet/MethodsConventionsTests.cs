namespace Application.Architecture.Tests.Conventions.DotNet;

public sealed class MethodsConventionsTests
{
    [Fact]
    public void Methods_should_be_PascalCased()
    {
        // Arrange
        var rules = Solution
            .TypesFromApplication.And()
            .AreClasses()
            .Should()
            .MeetCustomRule(new MethodsShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Async_methods_should_have_Async_suffix()
    {
        // Arrange
        var rules = Solution
            .TypesFromApplication.And()
            .AreClasses()
            .Should()
            .MeetCustomRule(new AsyncMethodsShouldHaveAsyncSuffixCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
