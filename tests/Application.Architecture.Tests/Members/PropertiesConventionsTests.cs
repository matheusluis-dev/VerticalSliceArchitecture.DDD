namespace Application.Architecture.Tests.Members;

/// <summary>
/// Test suite for validating <see langword="property"/> naming conventions in the application.
/// These tests ensure that <see langword="property"/> names adhere to .NET coding standards,
/// promoting consistency and readability throughout the codebase.
/// </summary>
public sealed class PropertiesConventionsTests
{
    /// <summary>
    /// <para>
    /// Ensures that all <see langword="property"/> names in the application follow the PascalCase
    /// naming convention.
    /// </para>
    ///
    /// <para>
    /// This practice aligns with .NET standards and improves code readability and maintainability.
    /// </para>
    /// </summary>
    [Fact]
    public void Properties_should_be_PascalCased()
    {
        // Arrange
        var rules = Sut
            .Types.Should()
            .MeetCustomRule(new PropertiesShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }
}
