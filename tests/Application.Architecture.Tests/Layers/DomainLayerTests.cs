namespace Application.Architecture.Tests.Layers;

/// <summary>
/// <para>
/// Test suite for validating dependency rules in the domain layer.
/// </para>
///
/// <para>
/// These tests ensure that the domain layer remains independent from other layers adhering to the
/// principles of clean architecture and ensuring core business logic is self-contained.
/// </para>
/// </summary>
public sealed class DomainLayerTests
{
    /// <summary>
    /// <para>
    /// Ensures that the domain layer does not have any dependencies outside of the <see cref="System"/>
    /// namespace.
    /// </para>
    ///
    /// <para>
    /// This rule enforces the independence of the domain layer, aligning with clean architecture
    /// principles.
    /// </para>
    /// </summary>
    [Fact]
    public void Domain_should_not_have_any_dependency()
    {
        // Arrange
        var infrastructureTypes = Sut.Types.WithInfrastructureTypesOnly().GetTypesFullName();
        var applicationTypes = Sut.Types.WithApplicationLayerTypesOnly().GetTypesFullName();

        var rules = Sut
            .Types.WithDomainTypesOnly()
            .Should()
            .NotHaveDependencyOnAny(infrastructureTypes)
            .And()
            .NotHaveDependencyOnAny(applicationTypes)
            .And()
            .OnlyHaveDependencyOn(Namespaces.System);

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }
}
