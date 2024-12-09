namespace Application.Architecture.Tests.Conventions.DotNet;

public sealed class InterfacesConventionsTests
{
    [Fact]
    public void Interfaces_should_be_PascalCased()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .AreInterfaces()
            .Should()
            .MeetCustomRule(new TypeShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Interfaces_should_start_with_I()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .AreInterfaces()
            .Should()
            .MeetCustomRule(new InterfacesShouldStartWithCapitalICustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
