namespace Application.Architecture.Tests.Conventions.Application;

using FastEndpoints;

/// <summary>
/// <para>
/// Test suite for validating naming and inheritance conventions related to
/// <see cref="FastEndpoints"/>.
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
    /// Ensures that all classes inheriting from <see cref="FastEndpoints"/>. types
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
        var rules = Solution
            .AllTypes.That()
            .Inherit(typeof(Endpoint<>))
            .Or()
            .Inherit(typeof(Endpoint<,>))
            .Or()
            .Inherit(typeof(Endpoint<,,>))
            .Or()
            .Inherit<EndpointWithoutRequest>()
            .Or()
            .Inherit(typeof(EndpointWithoutRequest<>))
            .Or()
            .Inherit(typeof(EndpointWithoutRequest<,>))
            .Should()
            .HaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
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
    public void Classes_outside_namespace_Endpoints_should_not_have_Endpoint_suffix()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .DoNotResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .Should()
            .NotHaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    /// <summary>
    /// <para>
    /// Verifies that classes within the "Endpoints" namespace that do not inherit from
    /// <see cref="FastEndpoints"/> types do not have an "Endpoint" suffix.
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
        var rules = Solution
            .AllTypes.That()
            .ResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .And()
            .DoNotInherit(typeof(Endpoint<>))
            .And()
            .DoNotInherit(typeof(Endpoint<,>))
            .And()
            .DoNotInherit(typeof(Endpoint<,,>))
            .And()
            .DoNotInherit<EndpointWithoutRequest>()
            .And()
            .DoNotInherit(typeof(EndpointWithoutRequest<>))
            .And()
            .DoNotInherit(typeof(EndpointWithoutRequest<,>))
            .Should()
            .NotHaveNameEndingWith("Endpoint");

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
