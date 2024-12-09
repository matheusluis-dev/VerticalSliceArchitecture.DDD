namespace Application.Architecture.Tests.Common.Extensions;

using System.Reflection;

public static class TypeExtensions
{
    public static MethodInfo[] GetUserDefinedMethods(this Type type)
    {
        if (type is null)
            return [];

        return type.GetMethods(
                BindingFlags.DeclaredOnly
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
            )
            .Where(m => m.IsUserDefined())
            .ToArray();
    }

    public static MethodInfo[] GetAsyncMethods(this Type type)
    {
        if (type is null)
            return [];

        return type.GetUserDefinedMethods().Where(m => m.IsAsynchronous()).ToArray();
    }
}
