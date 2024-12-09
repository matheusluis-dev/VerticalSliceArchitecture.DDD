namespace Application.Architecture.Tests.Common.Extensions;

using System.Reflection;
using System.Runtime.CompilerServices;

public static class MethodInfoExtensions
{
    public static bool IsAsynchronous(this MethodInfo methodInfo)
    {
        return methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>() is not null;
    }

    public static bool IsNotAsynchronous(this MethodInfo methodInfo)
    {
        return !methodInfo.IsAsynchronous();
    }

    public static bool IsUserDefined(this MethodInfo methodInfo)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        return !methodInfo.IsSpecialName
            && methodInfo.GetCustomAttribute<CompilerGeneratedAttribute>() is null;
    }

    public static bool IsNotUserDefined(this MethodInfo methodInfo)
    {
        return !methodInfo.IsUserDefined();
    }
}
