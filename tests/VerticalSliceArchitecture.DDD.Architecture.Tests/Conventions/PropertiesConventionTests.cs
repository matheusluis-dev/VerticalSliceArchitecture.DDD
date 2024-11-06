namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public sealed class PropertiesConventionTests
{
    [Fact]
    public void Properties_should_be_PascalCase()
    {
        // Arrange
        var classes = Solution.Types
            .That().AreClasses()
            .GetTypes();

        // Act
        var violations = classes.PropertiesWithNamesNotPascalCase();

        // Assert
        violations.Should().BeEmpty();
    }
}
