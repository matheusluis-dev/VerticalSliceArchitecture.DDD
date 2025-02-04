namespace Application.Architecture.Tests.Layers;

using Application.Architecture.Tests;

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
        var rules = SystemUnderTest
            .Types.That.ResideInNamespace("Application.Infrastructure")
            .Should.NotHaveDependencyOnNamespace("Application");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
