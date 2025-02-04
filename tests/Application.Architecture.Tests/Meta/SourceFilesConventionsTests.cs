namespace Application.Architecture.Tests.Meta;

/// <summary>
/// <para>
/// Test suite for validating architectural conventions related to source files.
/// </para>
///
/// <para>
/// These tests ensure consistency between type definitions and file organization, promoting
/// maintainability, readability, and adherence to project standards.
/// </para>
/// </summary>
public sealed class SourceFilesConventionsTests
{
    [Fact]
    public void Namespaces_should_match_folder_structure()
    {
        // Arrange
        var rules = SystemUnderTest.Types.Should.HaveSourceFilePathMatchingNamespace();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }

    /// <summary>
    /// <para>
    /// Ensures that each source file contains only one top-level <see langword="type"/>
    /// definition, unless additional type definitions are nested within another
    /// <see langword="type"/>.
    /// </para>
    ///
    /// <para>
    /// This test enforces modular file organization, simplifying navigation and preventing clutter
    /// in source files.
    /// </para>
    /// </summary>
    [Fact]
    public void Files_should_contain_only_one_type_definition_except_if_nested()
    {
        // TODO
    }

    /// <summary>
    /// <para>
    /// Verifies that the file name matches the name of the primary <see langword="type"/> defined
    /// within it.
    /// </para>
    ///
    /// <para>
    /// This convention simplifies locating <see langword="type"/>s within the codebase and aligns
    /// with standard file-naming practices, improving clarity and reducing confusion for
    /// developers.
    /// </para>
    /// </summary>
    [Fact]
    public void File_name_should_match_Type_name()
    {
        // Arrange
        var rules = SystemUnderTest.Types.Should.HaveSourceFileNameMatchingTypeName();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
