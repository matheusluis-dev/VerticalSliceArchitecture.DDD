namespace Application.Architecture.Tests.Common.Extensions;

using System.Reflection;

internal static class TypeDefinitionExtensions
{
    internal static bool IsStatic(this TypeDefinition typeDefinition)
    {
        return typeDefinition.IsSealed && typeDefinition.IsAbstract;
    }

    internal static bool IsNotStatic(this TypeDefinition typeDefinition)
    {
        return !typeDefinition.IsStatic();
    }

    internal static IEnumerable<string> GetNamespaceSplited(this TypeDefinition typeDefinition)
    {
        return typeDefinition.Namespace.Split('.');
    }

    public static MethodInfo[] GetUserDefinedMethods(this TypeDefinition typeDefinition)
    {
        if (typeDefinition is null)
            return [];

        return typeDefinition
            .ToType()
            .GetMethods(
                BindingFlags.DeclaredOnly
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
            )
            .Where(m => m.IsUserDefined())
            .ToArray();
    }

    public static MethodInfo[] GetAsyncMethods(this TypeDefinition typeDefinition)
    {
        if (typeDefinition is null)
            return [];

        return typeDefinition.GetUserDefinedMethods().Where(m => m.IsAsynchronous()).ToArray();
    }

    public static MethodInfo[] GetStaticMethods(this TypeDefinition typeDefinition)
    {
        if (typeDefinition is null)
            return [];

        return typeDefinition.GetUserDefinedMethods().Where(m => m.IsStatic).ToArray();
    }

    public static MethodInfo[] GetNonStaticMethods(this TypeDefinition typeDefinition)
    {
        if (typeDefinition is null)
            return [];

        return typeDefinition.GetUserDefinedMethods().Where(m => !m.IsStatic).ToArray();
    }
}
