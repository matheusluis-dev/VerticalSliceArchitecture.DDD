namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

internal static class MethodInfoExtensions
{
    public static bool IsAsync([NotNull] this MethodInfo methodInfo)
    {
        var asyncStateMachineAttribute =
            methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
        var isAsync = asyncStateMachineAttribute is not null;

        return isAsync;
    }
}
