namespace Application.Architecture.Tests.Common.Extensions;

using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class ClassDeclarationSyntaxExtensions
{
    internal static string GetNameWithNamespace(this ClassDeclarationSyntax classDeclarationSyntax)
    {
        var fullClassName = classDeclarationSyntax.Identifier.ValueText;

        var parent = classDeclarationSyntax.Parent;
        while (
            parent is not null and not BaseNamespaceDeclarationSyntax and not CompilationUnitSyntax
        )
        {
            // Treatment for Nested Classes
            if (parent is ClassDeclarationSyntax parentClass)
                fullClassName = $"{parentClass.Identifier.ValueText}.{fullClassName}";

            parent = parent.Parent;
        }

        if (parent is BaseNamespaceDeclarationSyntax namespaceDeclarationSyntax)
            fullClassName = $"{namespaceDeclarationSyntax.Name}.{fullClassName}";

        return fullClassName;
    }
}
