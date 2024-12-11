namespace Application.Architecture.Tests.Conventions.DotNet;

/// <summary>
/// Test suite for validating <see langword="method"/> naming conventions in the application.
/// These tests enforce adherence to .NET coding standards, ensuring consistency
/// and clarity in <see langword="method"/> names throughout the codebase.
/// </summary>
public sealed class MethodsConventionsTests
{
    /// <summary>
    /// <para>
    /// Ensures that all <see langword="method"/> names in the application follow the PascalCase
    /// naming convention.
    /// </para>
    ///
    /// <para>
    /// This standard improves readability and aligns with common .NET practices.
    /// </para>
    /// </summary>
    [Fact]
    public void Methods_should_be_PascalCased()
    {
        // Arrange
        var rules = Solution
            .TypesFromApplication.And()
            .AreClasses()
            .Should()
            .MeetCustomRule(new MethodsShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }

    /// <summary>
    /// <para>
    /// Validates that all asynchronous <see langword="method"/>s in the application have the
    /// "Async" suffix in their names.
    /// </para>
    ///
    /// <para>
    /// This convention makes the asynchronous nature of methods explicit and improves code
    /// clarity.
    /// </para>
    /// </summary>
    [Fact]
    public void Async_methods_should_have_Async_suffix()
    {
        // Arrange
        var rules = Solution
            .TypesFromApplication.And()
            .AreClasses()
            .Should()
            .MeetCustomRule(new AsyncMethodsShouldHaveAsyncSuffixCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful();
    }
}
