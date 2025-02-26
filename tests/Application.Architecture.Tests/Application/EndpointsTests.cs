using FastEndpoints;

namespace Application.Architecture.Tests.Application;

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
public sealed class EndpointsTests
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
        var rules = SystemUnderTest
            .Types.That.Inherit(typeof(Endpoint<>), typeof(Endpoint<,>), typeof(Endpoint<,,>))
            .Should.HaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
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
        var rules = SystemUnderTest
            .Types.That.DoNotResideInNamespace("Application.Endpoints")
            .Should.NotHaveNameEndingWith("Endpoint")
            .And.NotInherit(typeof(Endpoint<>), typeof(Endpoint<,>), typeof(Endpoint<,,>));

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
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
        var rules = SystemUnderTest
            .Types.That.ResideInNamespace("Application.Endpoints")
            .And.DoNotInherit(typeof(Endpoint<>), typeof(Endpoint<,>), typeof(Endpoint<,,>))
            .Should.NotHaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
    }
}
