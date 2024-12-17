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
        // Arrange
        var failingIdentifiers = new FailingIdentifiers();

        // Act
        var filesWithMoreThan1Type = ApplicationCSharpFilesHelper
            .Files.Where(file =>
            {
                var code = File.ReadAllText(file.FullName);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetRoot();

                var topLevelTypeDeclarations = root.DescendantNodes()
                    .OfType<TypeDeclarationSyntax>()
                    // If type.Parent is BaseNamespaceDeclarationSyntax, means it's not nested.
                    .Where(type =>
                        type.Parent is BaseNamespaceDeclarationSyntax @namespace
                        // Rules do not apply to Endpoints
                        && (
                            type.Identifier.Text.EndsWith("Endpoint", StringComparison.Ordinal)
                            || !@namespace
                                .Name.ToString()
                                .StartsWith(
                                    Namespaces.ApplicationLayer.Endpoints,
                                    StringComparison.Ordinal
                                )
                        )
                    );

                return topLevelTypeDeclarations.Skip(1).Any();
            })
            .Select(file => file.FullName)
            .ToList();

        failingIdentifiers.AddRange(filesWithMoreThan1Type);

        // Assert
        failingIdentifiers.Should().NotContainFailures();
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
        var rules = Sut
            .Types.That()
            .DoNotResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .Or()
            .HaveNameEndingWith("Endpoint")
            .Should()
            .HaveSourceFileNameMatchingName();

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }
}
