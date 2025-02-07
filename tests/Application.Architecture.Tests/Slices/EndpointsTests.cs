namespace Application.Architecture.Tests.Slices;

public sealed class EndpointsTests
{
    [Fact]
    public void Vertical_slices_from_Endpoints_should_not_depend_on_each_other()
    {
        // Arrange
        var rules = SystemUnderTest
            .Types.Slice.ByNamespacePrefix("Application.Endpoints")
            .Should.NotHaveDependenciesBetweenSlices();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
    }
}
