namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public sealed class MethodsConventionTests
{
    private const string Async = nameof(Async);

    [Fact]
    public void AsyncMethodsShouldHaveAsyncSuffix()
    {
        Application
            .ClassesWithAsyncMethods.Should()
            .AllSatisfy(@class =>
                @class
                    .AsyncMethods.Should()
                    .AllSatisfy(asyncMethod => asyncMethod.Name.Should().EndWith(Async))
            );
    }
}
