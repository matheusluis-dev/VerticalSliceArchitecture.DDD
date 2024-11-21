namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class PartialClassFinder
{
    internal static IEnumerable<string> GetPartialClassNames()
    {
        var directoryPath = @"C:\Users\mathe\source\github\me\VerticalSliceArchitecture.DDD\src\VerticalSliceArchitecture.DDD.Application";

        var csFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            var code = File.ReadAllText(file);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classDeclaration in classDeclarations)
            {
                if (classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                {
                    yield return classDeclaration.Identifier.Text;
                }
            }
        }
    }
}
