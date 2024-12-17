namespace Application.Architecture.Tests.Members;

/// <summary>
/// <para>
/// Test suite for validating field naming and accessibility conventions in the application.
/// </para>
///
/// <para>
/// These tests enforce adherence to common .NET coding standards for private and
/// non-<see langword="static"/> fields ensuring better maintainability and code quality.
/// </para>
/// </summary>
public sealed class FieldsConventionsTests
{
    /// <summary>
    /// <para>
    /// Ensures that <see langword="private"/> fields in the application follow the naming
    /// convention of being _camelCased with an underscore prefix.
    /// </para>
    ///
    /// <para>
    /// This standard improves code readability and aligns with .NET best practices.
    /// </para>
    /// </summary>
    [Fact]
    public void Private_fields_should_be_camelCased_with_underscore_prefix()
    {
        // Arrange
        var rules = Sut
            .Types.Should()
            .MeetCustomRule(new PrivateFieldsShouldBeCamelCasedWithUnderscorePrefixCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }

    /// <summary>
    /// <para>
    /// Validates that non-<see langword="static"/> fields are not declared as
    /// <see langword="public"/> in the codebase.
    /// </para>
    ///
    /// <para>
    /// This rule helps to maintain encapsulation and promotes better design practices.
    /// </para>
    /// </summary>
    [Fact]
    public void Non_static_fields_should_not_be_public()
    {
        // Arrange
        var rules = Sut.Types.Should().MeetCustomRule(new FieldsShouldNotBePublicCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }

    [Fact]
    public void Non_private_static_fields_should_be_PascalCased()
    {
        // Arrange
        var rules = Sut
            .Types.Should()
            .MeetCustomRule(new NonPrivateStaticFieldsShouldBePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }
}
