namespace Application.Architecture.Tests.Conventions.Application;

using FastEndpoints;

public sealed class EndpointsConventionsTests
{
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
            .Should()
            .MeetCustomRule(new ShouldHaveSuffixCustomRule("Endpoint"));

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Classes_outside_namespace_Endpoints_should_not_have_Endpoint_suffix()
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .DoNotResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .ShouldNot()
            .MeetCustomRule(new ShouldHaveSuffixCustomRule("Endpoint"));

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public void Classes_inside_namespace_Endpoints_that_does_not_inherit_from_Endpoint_class_should_not_have_Endpoint_suffix()
    {
        var rules = Solution
            .AllTypes.That()
            .ResideInNamespace(Namespaces.ApplicationLayer.Endpoints)
            .And()
            .DoNotInherit(typeof(Endpoint<>))
            .And()
            .DoNotInherit(typeof(Endpoint<,>))
            .And()
            .DoNotInherit(typeof(Endpoint<,,>))
            .ShouldNot()
            .MeetCustomRule(new ShouldHaveSuffixCustomRule("Endpoint"));

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
