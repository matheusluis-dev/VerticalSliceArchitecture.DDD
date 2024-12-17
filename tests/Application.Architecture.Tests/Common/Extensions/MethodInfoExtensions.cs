namespace Application.Architecture.Tests.Common.Extensions;

using System.Reflection;
using System.Reflection.Emit;
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

    public static bool IsMethodBodyEmpty(this MethodInfo methodInfo)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        // Get the method body
        var methodBody = methodInfo.GetMethodBody();
        if (methodBody is null)
            return true; // Methods like abstract, interface, or external (P/Invoke) have no body.

        // Get the IL byte array
        var ilBytes = methodBody.GetILAsByteArray();
        if (ilBytes == null || ilBytes.Length == 0)
            return true;

        // Check if the IL contains only a `ret` instruction (0x2A)
        return ilBytes.All(b =>
            b == OpCodes.Nop.Value || b == OpCodes.Ret.Value || b == OpCodes.Br_S.Value
        );
    }

    public static bool IsMethodBodyNotEmpty(this MethodInfo methodInfo)
    {
        return !methodInfo.IsMethodBodyEmpty();
    }
}
