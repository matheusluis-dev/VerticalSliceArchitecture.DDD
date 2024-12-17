namespace Application.Architecture.Tests.Meta;

public sealed class NamespacesConventionsTests
{
    [Fact]
    public void Namespaces_should_be_PascalCased()
    {
        // Arrange
        var rules = Sut
            .Types.Should()
            .MeetCustomRule(new NamespaceShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }

    [Fact]
    public void Namespaces_should_match_folder_structure()
    {
        // Arrange
        var rules = Sut.Types.Should().HaveSourceFilePathMatchingNamespace();

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful("Namespaces should match folder structure path");
    }
}
