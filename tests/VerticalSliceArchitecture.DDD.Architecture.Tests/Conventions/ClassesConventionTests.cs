namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;
using VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Extensions;

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
    public void NonAbstractNonStaticNonPartialClassesShouldBeSealed()
    {
        Application
            .NonAbstractNonStaticNonPartialClasses.Should()
            .AllSatisfy(@class => @class.Should().BeSealed());
    }

    /// <summary>
    /// Ensures that all classes with exclusively static methods are marked as static to adhere
    /// to design best practices and communicate intent clearly.
    /// <para>
    /// Rationale: Declaring a class as static when all its methods are static reinforces the
    /// class's purpose as a utility or helper. It prevents accidental instantiation and signals
    /// that the class is not meant to maintain any instance-level state.
    /// </para>
    /// </summary>
    [Fact]
    public void ClassesThatAllMethodsAreStaticShouldBeStatic()
    {
        Application
            .NonStaticClasses.Should()
            .AllSatisfy(@class =>
                @class.GetMethods().Length.Should().NotBe(@class.GetStaticMethods().Count())
            );
    }
}
