namespace Application.Architecture.Tests.Dependencies;

public sealed class NewtonsoftJsonTests
{
    [Fact]
    public void NewtonsoftJson_should_not_be_used()
    {
        // Arrange
        var rules = Sut.Types.Should().NotHaveDependencyOnAny("Newtonsoft.Json");

        // Act
        var result = rules.GetResult();

        // Assert
        result
            .Should()
            .BeSuccessful("[Newtonsoft.Json] should not be used, use [System.Text.Json] instead");
    }
}
