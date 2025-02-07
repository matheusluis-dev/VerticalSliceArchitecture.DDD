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
public sealed class FieldsTests
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
    public void Private_and_protected_fields_should_be_camelCased_with_underscore_prefix()
    {
        // Arrange
        var rules = SystemUnderTest
            .Types.Verify()
            .Fields.That.AreNotConst()
            .And(fields => fields.ArePrivate().Or.AreProtected())
            .Should.HaveNameCamelCased('_');

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
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
    public void Non_static_fields_should_not_be_public_and_not_be_internal()
    {
        // Arrange
        var rules = SystemUnderTest
            .Types.Verify()
            .Fields.That.AreNotStatic()
            .Should.NotBePublic()
            .And.NotBeInternal();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
    }

    [Fact]
    public void Static_fields_that_are_not_private_or_protected_should_have_name_PascalCased()
    {
        // Arrange
        var rules = SystemUnderTest
            .Types.Verify()
            .Fields.That.AreStatic()
            .And.AreNotProtected()
            .And.AreNotPrivate()
            .Should.HaveNamePascalCased();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
    }
}
