namespace Application.Architecture.Tests.Layers;

public sealed class DomainLayerTests
{
    [Fact]
    public void Domain_should_not_have_any_dependency()
    {
        // Arrange
        var rules = Solution.DomainTypes.Should().OnlyHaveDependencyOn(Namespaces.System);

        // Act
        var result = rules.GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
