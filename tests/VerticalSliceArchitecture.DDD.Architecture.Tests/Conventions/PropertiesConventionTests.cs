namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;
using VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Extensions;

public sealed class PropertiesConventionTests
{
    /// <summary>
    /// Validates that all properties follow PascalCase naming convention to ensure consistency
    /// and adherence to .NET coding standards.
    /// <para>
    /// Rationale: Using PascalCase for property names aligns with widely accepted .NET guidelines,
    /// improves code readability, and maintains a uniform style across the codebase. This also
    /// reduces potential misunderstandings or errors caused by inconsistent naming.
    /// </para>
    /// </summary>
    [Fact]
    public void PropertiesShouldBePascalCase()
    {
        Application
            .ClassesWithProperties.Should()
            .AllSatisfy(@class =>
                @class.Properties.Should().AllSatisfy(property => property.Name.IsPascalCase())
            );
    }
}
