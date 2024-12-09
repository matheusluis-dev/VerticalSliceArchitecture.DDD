namespace Application.Architecture.Tests.Slices;

public sealed class DomainVerticalSlicesTests
{
    [Fact]
    public void Vertical_slices_from_Domain_should_not_depend_on_each_other()
    {
        // Arrange
        var rules = Solution
            .AllTypes.Slice()
            .ByNamespacePrefix(Namespaces.DomainLayer.Features)
            .Should()
            .NotHaveDependenciesBetweenSlices();

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
