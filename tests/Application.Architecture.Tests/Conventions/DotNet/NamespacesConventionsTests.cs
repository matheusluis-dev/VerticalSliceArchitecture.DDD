namespace Application.Architecture.Tests.Conventions.DotNet;

public sealed class NamespacesConventionsTests
{
    [Fact]
    public void Namespaces_should_be_PascalCased()
    {
        // Arrange
        var rules = Solution
            .AllTypes.Should()
            .MeetCustomRule(new NamespaceShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Namespaces_should_match_folder_structure()
    {
        // Arrange
        var rules = Solution.AllTypes.Should().HaveSourceFilePathMatchingNamespace();

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful("Namespaces should match folder structure path.");
    }
}
