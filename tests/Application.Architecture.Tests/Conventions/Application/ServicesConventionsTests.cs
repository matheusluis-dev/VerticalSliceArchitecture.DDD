namespace Application.Architecture.Tests.Conventions.Application;

public sealed class ServicesConventionsTests
{
    [Fact]
    public void Services_classes_should_have_Service_suffix()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .ResideInNamespace(Namespaces.ApplicationLayer.Services)
            .Should()
            .MeetCustomRule(new ShouldHaveSuffixCustomRule("Service"));

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Classes_outside_namespace_Services_should_not_have_Service_suffix()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .DoNotResideInNamespace(Namespaces.ApplicationLayer.Services)
            .ShouldNot()
            .MeetCustomRule(new ShouldHaveSuffixCustomRule("Service"));

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
