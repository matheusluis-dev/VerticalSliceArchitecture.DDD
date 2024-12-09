namespace Application.Architecture.Tests.Slices;

public sealed class EndpointsVerticalSlicesTests
{
    [Fact]
    public void Vertical_slices_from_Endpoints_should_not_depend_on_each_other()
    {
        // Arrange
        var rules = Solution
            .AllTypes.Slice()
            .ByNamespacePrefix(Namespaces.ApplicationLayer.Endpoints)
            .Should()
            .NotHaveDependenciesBetweenSlices();

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
