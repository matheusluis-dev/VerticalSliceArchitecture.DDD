namespace Application.Architecture.Tests.Members;

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
        var rules = Sut
            .Types.That()
            .AreClasses()
            .Should()
            .MeetCustomRule(new MethodsShouldHaveNamePascalCasedCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
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
        var rules = Sut
            .Types.That()
            .AreClasses()
            .Should()
            .MeetCustomRule(new AsyncMethodsShouldHaveAsyncSuffixCustomRule());

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }

    [Fact]
    public void Async_methods_should_return_Task_CompletedTask_instead_of_empty_return()
    {
        // Arrange
        var rules = Sut
            .Classes.Should()
            .MeetCustomRule(
                new AsyncMethodsShouldReturnTaskCompletedTaskInsteadOfEmptyReturnCustomRule()
            );

        // Act
        var result = rules.GetResult();

        // Assert
        result.Should().BeSuccessful();
    }
}