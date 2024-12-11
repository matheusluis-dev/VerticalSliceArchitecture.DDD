namespace Application.Architecture.Tests.Common.Helpers;

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class PartialClassesHelper
{
    private static readonly Lazy<IEnumerable<string>> _cache = new(LoadPartialClasses);

    internal static IEnumerable<string> Types => _cache.Value;

    private static IEnumerable<string> LoadPartialClasses()
    {
        var directoryInfo = DirectoryHelper.GetDirectoryInSolution(Paths.Application);

        return directoryInfo
            .EnumerateFiles("*.cs", SearchOption.AllDirectories)
            .SelectMany(file =>
            {
                var code = File.ReadAllText(file.FullName);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetRoot();

                return root.DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(c => c.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                    .Select(c => c.Identifier.Text);
            });
    }
}
