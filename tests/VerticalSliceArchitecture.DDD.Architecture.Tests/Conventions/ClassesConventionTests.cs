namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public sealed class ClassesConventionTests
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
        var nonAbstractNonStaticNonPartialClasses = Application.Classes
            .Where(c => !c.IsAbstract && c.IsNotStatic() && c.IsNotPartial());

        // Assert
        nonAbstractNonStaticNonPartialClasses.Should()
            .AllSatisfy(@class => @class.Should().BeSealed());
    }

    [Fact]
    public void Classes_that_all_methods_are_static_should_be_static()
    {
        // Arrange
        var staticMethods = Application.StaticMethods
            .Where(sm => sm.Methods.All(m => m.IsStatic))
            .Where(sm => sm.Class.ReflectedType!.IsNotStatic());

        // Assert
        staticMethods.Should().BeEmpty();
    }
}
