using System;
using System.Runtime.CompilerServices;
using VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Conventions;

public sealed class MethodsConventionTests
{
    private const string Async = nameof(Async);

    [Fact]
    public void Async_methods_should_have_Async_suffix()
    {
        // Arrange
        var asyncMethods = Common.Application.AsyncMethods;

        // Assert
        asyncMethods.Should().AllSatisfy(am =>
            am.Methods.Should().AllSatisfy(methodInfo => methodInfo.Name.Should().EndWith(Async)));
    }
}
