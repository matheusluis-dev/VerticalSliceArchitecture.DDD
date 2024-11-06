namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;
using VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Rules;

public sealed class ClassConventionTests
{
    /// <summary>
    /// Verifies that all non-abstract, non-static, non-partial classes are sealed to enforce immutability 
    /// and prevent unintended inheritance.
    /// <para>
    /// Rationale: Sealing classes improves performance, reduces complexity, and signals that 
    /// a class is designed for direct use without extension. Additionally, classes that are 
    /// non-partial are easier to understand and maintain as a single, cohesive unit.
    /// </para>
    /// </summary>
    [Fact]
    public void Non_abstract_non_static_non_partial_classes_should_be_sealed()
    {
        // Arrange
        var nonAbstractNonStaticNonPartialClasses = Solution.Types
            .That().AreClasses()
            .And().AreNotAbstract()
            .And().AreNotStatic()
            .And().MeetCustomRule(new AreNotPartialCustomRule());

        // Assert
        nonAbstractNonStaticNonPartialClasses.Should().BeSealed();
    }
}
