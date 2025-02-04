namespace Application.Architecture.Tests.Infrastructure;

public sealed class InfrastructureServicesTests
{
    [Fact]
    public void Infrastructure_services_should_have_Service_suffix()
    {
        // Arrange
        var rules = SystemUnderTest
            .Types.That.ResideInNamespace("Application.Infrastructure.Services")
            .Should.HaveNameEndingWith("Service");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
