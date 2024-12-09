namespace Application.Architecture.Tests.Layers;

public sealed class InfrastructureLayerTests
{
    [Fact]
    public void Infrastructure_should_not_depend_on_Application()
    {
        // Arrange
        var applicationTypesFullName = Solution.NonDomainNonInfrastructureTypes.GetTypesFullName();

        var rules = Solution
            .InfrastructureTypes.Should()
            .NotHaveDependencyOnAny(applicationTypesFullName);

        // Act
        var result = rules.GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
