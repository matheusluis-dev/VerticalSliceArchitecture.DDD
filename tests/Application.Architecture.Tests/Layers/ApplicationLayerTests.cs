namespace Application.Architecture.Tests.Layers;

public sealed class ApplicationLayerTests
{
    [Fact]
    public void Application_should_not_depend_on_Infrastructure()
    {
        // Arrange
        var infrastructureTypesFullName = Solution.InfrastructureTypes.GetTypesFullName();

        var rules = Solution
            .NonDomainNonInfrastructureTypes.Should()
            .NotHaveDependencyOnAny(infrastructureTypesFullName);

        // Act
        var result = rules.GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
