namespace Application.Architecture.Tests.Dependencies;

public sealed class LegacyLibrariesTests
{
    [Theory]
    [ClassData(typeof(Libraries))]
    public void Application_should_not_use_legacy_libraries(
        (string Legacy, string Alternative) tuple
    )
    {
        // Arrange
        var rules = Solution.AllTypes.Should().NotHaveDependencyOnAny(tuple.Legacy);

        // Act
        var result = rules.GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue($"Use {tuple.Alternative} instead of {tuple.Legacy}");
    }
}
