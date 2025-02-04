namespace Application.Architecture.Tests.Types;

/// <summary>
/// <para>
/// Test suite for validating architectural conventions related to <see langword="interface"/>
/// definitions.
/// </para>
///
/// <para>
/// These tests enforce naming standards for interfaces, promoting clarity, consistency, and
/// alignment with widely accepted development practices.
/// </para>
/// </summary>
public sealed class InterfacesConventionsTests
{
    /// <summary>
    /// Verifies that all <see langword="interface"/> in the solution follow the PascalCase naming
    /// convention.
    /// This ensures that <see langword="interface"/> names are consistent, readable, and align
    /// with established .NET naming guidelines, facilitating better collaboration and
    /// maintainability.
    /// </summary>
    [Fact]
    public void Interfaces_should_be_PascalCased()
    {
        // Arrange
        var rules = SutArchGuard.Types.That.AreInterfaces().Should.HaveNamePascalCased();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    /// <summary>
    /// <para>
    /// Ensures that all <see langword="interface"/> names begin with the letter "I" to clearly
    /// distinguish <see langword="interface"/>s from other types in the codebase.
    ///</para>
    ///
    /// <para>
    /// This convention simplifies code comprehension and aligns with common .NET standards.
    /// </para>
    /// </summary>
    [Fact]
    public void Interfaces_should_start_with_I_followed_by_a_capital_letter()
    {
        // Arrange
        var rules = SutArchGuard.Types.That.AreInterfaces().Should.HaveNameMatching("^I[A-Z]");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
