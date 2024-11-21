using System.Runtime.CompilerServices;

namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public static class MethodInfoExtensions
{
    public static bool IsAsync(this MethodInfo methodInfo)
    {
        var asyncStateMachineAttribute = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
        var isAsync = asyncStateMachineAttribute is not null;

        return isAsync;
    }
}
