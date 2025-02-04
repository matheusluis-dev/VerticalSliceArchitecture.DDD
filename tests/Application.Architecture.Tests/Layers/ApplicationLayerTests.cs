namespace Application.Architecture.Tests.Layers;

/// <summary>
/// <para>
/// Test suite for validating dependency rules in the application layer.
/// </para>
///
///<para>
/// These tests enforce architectural boundaries, ensuring that the application layer remains
/// decoupled from the infrastructure layer to promote maintainability and scalability.
/// </para>
/// </summary>
public sealed class ApplicationLayerTests
{
    /// <summary>
    /// <para>
    /// Ensures that the application layer does not depend on the infrastructure layer.
    /// </para>
    ///
    /// <para>
    /// This rule enforces a clean separation of concerns, aligning with best practices in layered
    /// architecture.
    /// </para>
    /// </summary>
    [Fact]
    public void Application_should_not_depend_on_Infrastructure()
    {
        // Arrange
        var rules = SutArchGuard
            .Types.That.DoNotResideInNamespace("Application.Domain", "Application.Infrastructure")
            .Should.NotHaveDependencyOnNamespace(
                "Application.Domain",
                "Application.Infrastructure"
            );

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
