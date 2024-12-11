namespace Application.Architecture.Tests.Common.Extensions;

internal static class TypeDefinitionExtensions
{
    public static bool IsStatic(this TypeDefinition typeDefinition)
    {
        return typeDefinition.IsSealed && typeDefinition.IsAbstract;
    }

    public static bool IsNotStatic(this TypeDefinition typeDefinition)
    {
        return !typeDefinition.IsStatic();
    }

    public static IEnumerable<string> GetNamespaceSplited(this TypeDefinition typeDefinition)
    {
        return typeDefinition.Namespace.Split('.');
    }
}
