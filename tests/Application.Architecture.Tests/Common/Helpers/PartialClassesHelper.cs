namespace Application.Architecture.Tests.Common.Helpers;

internal static class PartialClassesHelper
{
    private static readonly Lazy<IEnumerable<string>> _cache = new(LoadPartialClasses);

    internal static IEnumerable<string> Classes => _cache.Value;

    private static IEnumerable<string> LoadPartialClasses()
    {
        return ApplicationCSharpFilesHelper.Files.SelectMany(file =>
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
