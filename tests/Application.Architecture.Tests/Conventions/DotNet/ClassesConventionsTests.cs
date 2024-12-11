namespace Application.Architecture.Tests.Conventions.DotNet;

/// <summary>
/// <para>
/// Test suite for validating adherence to architectural conventions for class definitions.
/// </para>
///
/// <para>
/// These tests enforce key design principles and naming standards to maintain code quality,
/// consistency, and scalability across the solution.
/// </para>
/// </summary>
public sealed class ClassesConventionsTests
{
    /// <summary>
    /// <para>
    /// Verifies that all class names conform to the PascalCase naming convention.
    /// </para>
    ///
    /// <para>
    /// Adhering to this standard ensures a consistent naming strategy, improving code readability
    /// and alignment with widely accepted best practices, facilitating easier collaboration and
    /// maintenance.
    /// </para>
    /// </summary>
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

    /// <summary>
    /// <para>
    /// Ensures that all non-abstract, non-static, and non-partial classes are declared as sealed.
    /// </para>
    ///
    /// <para>
    /// This test promotes better design by preventing unintended inheritance, ensuring that
    /// classes are explicitly designed for extension if needed. It helps reduce complexity and
    /// potential errors caused by overusing inheritance.
    /// </para>
    /// </summary>
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
}
