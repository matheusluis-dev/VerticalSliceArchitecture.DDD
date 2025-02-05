namespace Application.Architecture.Tests.Dependencies;

public sealed class NewtonsoftJsonTests
{
    [Fact]
    public void NewtonsoftJson_should_not_be_used()
    {
        // Arrange
        var rules = SystemUnderTest.Types.Should.NotHaveDependencyOnNamespace("Newtonsoft");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        result.ShouldBeSuccess();
    }
}
