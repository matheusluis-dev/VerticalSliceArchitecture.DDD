namespace Application.Architecture.Tests.Conventions.DotNet;

public sealed class ClassesConventionsTests
{
    [Fact]
    public void Non_Abstract_non_Static_non_Partial_classes_should_be_Sealed()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .AreClasses()
            .Should()
            .MeetCustomRule(new NonAbstractNonStaticNonPartialClassesShouldBeSealedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Classes_should_be_PascalCased()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .AreClasses()
            .Should()
            .MeetCustomRule(new TypeShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
