namespace Application.Architecture.Tests.Dependencies;



public sealed class NewtonsoftJsonTests
{
    [Fact]
    public void NewtonsoftJson_should_not_be_used()
    {
        // Arrange
        var rules = SutArchGuard.Types.Should.NotHaveDependencyOn("Newtonsoft.Json");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
