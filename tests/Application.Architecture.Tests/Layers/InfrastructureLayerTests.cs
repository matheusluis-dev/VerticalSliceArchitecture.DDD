namespace Application.Architecture.Tests.Layers;

/// <summary>
/// <para>
/// Test suite for validating dependency rules in the infrastructure layer.
/// </para>
///
/// <para>
/// These tests ensure that the infrastructure layer does not depend on the application layer,
/// preserving the intended architectural boundaries and promoting a clean separation of concerns.
/// </para>
/// </summary>
public sealed class InfrastructureLayerTests
{
    /// <summary>
    /// <para>
    /// Ensures that the infrastructure layer does not depend on the application layer.
    /// </para>
    ///
    /// <para>
    /// This rule enforces the architectural boundary between these layers, maintaining clear
    /// responsibilities.
    /// </para>
    /// </summary>
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
