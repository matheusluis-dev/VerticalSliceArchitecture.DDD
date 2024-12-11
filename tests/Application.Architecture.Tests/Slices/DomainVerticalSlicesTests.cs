namespace Application.Architecture.Tests.Slices;

/// <summary>
/// Test suite for validating vertical slice independence within the domain layer.
/// These tests ensure that features in the domain layer are self-contained and do not depend on each other,
/// adhering to the principles of Vertical Slice Architecture and promoting modularity.
/// </summary>
public sealed class DomainVerticalSlicesTests
{
    /// <summary>
    /// Ensures that vertical slices within the domain layer do not depend on each other.
    /// This rule promotes modularity and independence, allowing each feature to evolve independently.
    /// </summary>
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
