namespace Application.Architecture.Tests.Dependencies;

/// <summary>
/// <para>
/// Test suite for validating <see cref="HttpClient"/> related types usage within the application.
/// </para>
///
/// <para>
/// These tests enforce best practices for managing dependencies, promoting scalability,
/// maintainability, and adherence to modern design principles.
/// </para>
/// </summary>
public sealed class HttpClientTests
{
    /// <summary>
    /// <para>
    /// Ensures that the <see cref="HttpClient"/> class is not instantiated directly in the
    /// codebase.
    /// </para>
    ///
    /// <para>
    /// Instead, <see cref="IHttpClientFactory"/> should be used to manage and configure
    /// <see cref="HttpClient"/> instances.
    /// </para>
    ///
    /// <para>
    /// This practice helps prevent issues like socket exhaustion, improves testability, and aligns
    /// with recommended dependency management patterns in .NET.
    /// </para>
    ///
    /// <para>
    /// > <see href="https://stackoverflow.com/questions/58427764"/>
    /// </para>
    /// </summary>
    [Fact]
    public void HttpClient_should_not_be_instantiated_directly()
    {
        // Arrange
        var rules = SystemUnderTest.Types.Should.NotHaveDependencyOn("System.Net.Http.HttpClient");

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }
}
