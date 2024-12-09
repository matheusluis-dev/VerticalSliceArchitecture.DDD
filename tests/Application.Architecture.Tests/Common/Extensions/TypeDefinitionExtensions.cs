namespace Application.Architecture.Tests.Common.Extensions;

internal static class TypeDefinitionExtensions
{
    public static bool IsStatic(this TypeDefinition typeDefinition)
    {
        return typeDefinition.IsSealed && typeDefinition.IsAbstract;
    }
}
