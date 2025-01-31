namespace Application.Architecture.Tests.Conventions.Application;

using Carter;
using NFluent;

/// <summary>
/// <para>
/// Test suite for validating naming and inheritance conventions related to
/// Endpoints.
/// </para>
///
/// <para>
/// These tests ensure consistency in class naming and adherence to architectural guidelines,
/// improving maintainability and clarity in the application.
/// </para>
/// </summary>
public sealed class EndpointsConventionsTests
{
    /// <summary>
    /// <para>
    /// Ensures that all classes inheriting from Endpoints. types
    /// (Endpoint, Endpoint with generics have an "Endpoint" suffix in their names.
    /// </para>
    ///
    /// <para>
    /// This convention improves code readability and uniformity.
    /// </para>
    /// </summary>
    [Fact]
    public void Endpoints_classes_should_have_Endpoint_suffix()
    {
        // Arrange
        var rules = SutArchGuard
            .Types.That.ImplementInterface(typeof(ICarterModule))
            .Should.HaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }

    /// <summary>
    /// <para>
    /// Validates that classes residing outside the "Endpoints" namespace do not have an "Endpoint"
    /// suffix.
    /// </para>
    ///
    /// <para>
    /// This ensures proper scoping and avoids confusion with FastEndpoints types.
    /// </para>
    /// </summary>
    [Fact]
    public void Classes_outside_namespace_Endpoints_should_not_be_an_Endpoint()
    {
        // Arrange
        var rules = SutArchGuard
            .Types.That.DoNotResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .Should.NotHaveNameEndingWith("Endpoint")
            .And.NotImplementInterface(typeof(ICarterModule));

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }

    /// <summary>
    /// <para>
    /// Verifies that classes within the "Endpoints" namespace that do not inherit from
    /// Endpoints types do not have an "Endpoint" suffix.
    /// </para>
    ///
    /// <para>
    /// This rule prevents misuse of naming conventions.
    /// </para>
    /// </summary>
    [Fact]
    public void Classes_inside_namespace_Endpoints_that_does_not_inherit_from_Endpoint_class_should_not_have_Endpoint_suffix()
    {
        // Arrange
        var rules = SutArchGuard
            .Types.That.ResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .And.ImplementInterface(typeof(ICarterModule))
            .Should.NotHaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
