namespace Application.Architecture.Tests.Conventions.Application;

public sealed class ServicesConventionsTests
{
    [Fact]
    public void Services_classes_should_have_Service_suffix()
    {
        // Arrange
        var rules = SutArchGuard
            .Types.That.ResideInNamespace(Namespaces.ApplicationLayer.Services)
            .Should.HaveNameEndingWith("Service");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }

    [Fact]
    public void Classes_outside_namespace_Services_should_not_have_Service_suffix()
    {
        // Arrange
        var rules = SutArchGuard
            .Types.That.DoNotResideInNamespace(Namespaces.ApplicationLayer.Services)
            .Should.NotHaveNameEndingWith("Service");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
