namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public sealed class InterfaceConventionsTests
{
    /// <summary>
    /// Validates that all interface names in the solution follow the standard naming convention.
    /// <para>
    /// Specifically, this test checks that each interface name starts with an uppercase "I" 
    /// followed by another uppercase letter. This ensures consistency and helps prevent 
    /// naming errors where a type could be mistakenly named as a regular class (e.g., "Integrator") 
    /// instead of a proper interface name (e.g., "IIntegrator").
    /// </para>
    /// <para>
    /// Rationale: Testing only for a leading "I" could result in false positives, allowing names 
    /// that do not clearly indicate an interface. By enforcing a pattern of "I" followed by 
    /// another uppercase character, we strengthen code readability and align with widely 
    /// accepted C# naming conventions.
    /// </para>
    /// </summary>
    [Fact]
    public void Interfaces_starts_with_I()
    {
        // Arrange
        var interfaces = Solution.Types
            .That().AreInterfaces()
            .GetTypes()
            .Select(i => i.Name);

        // Assert
        foreach (var name in interfaces)
        {
            name.Should().StartWith("I");
            name.Length.Should().BeGreaterThan(1);
            name[1].ToString().Should().BeUpperCased();
        }
    }
}
