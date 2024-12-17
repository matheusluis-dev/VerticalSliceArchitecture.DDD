namespace Application.Architecture.Tests.Common.Helpers;

public static class ConstsHelper
{
    private static readonly Lazy<IEnumerable<ConstDefinition>> _cache = new(LoadConsts);

    internal static IEnumerable<ConstDefinition> Consts => _cache.Value;

    private static IEnumerable<ConstDefinition> LoadConsts()
    {
        return ApplicationCSharpFilesHelper.Files.SelectMany(file =>
        {
            var code = File.ReadAllText(file.FullName);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            var constDefinitions = new List<ConstDefinition>();
            foreach (var @class in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                constDefinitions.AddRange(
                    @class
                        .DescendantNodes()
                        .OfType<FieldDeclarationSyntax>()
                        .Where(field => field.Modifiers.Any(SyntaxKind.ConstKeyword))
                        .SelectMany(field => field.Declaration.Variables)
                        .Select(@const => new ConstDefinition
                        {
                            LocationType = "Class",
                            LocationName = @class.GetNameWithNamespace(),
                            ConstName = @const.Identifier.ValueText,
                            ConstValue = @const.Initializer?.Value.ToString(),
                        })
                );

                LoadConstsFromMethods(@class, constDefinitions);
            }

            return constDefinitions;
        });
    }

    private static void LoadConstsFromMethods(
        ClassDeclarationSyntax @class,
        List<ConstDefinition> constDefinitions
    )
    {
        foreach (var method in @class.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            var methodNameWithFullClassName =
                $"{@class.GetNameWithNamespace()}.{method.Identifier.ValueText}()";

            constDefinitions.AddRange(
                method
                    .DescendantNodes()
                    .OfType<LocalDeclarationStatementSyntax>()
                    .Where(localDeclaration =>
                        localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword)
                    )
                    .SelectMany(localDeclaration => localDeclaration.Declaration.Variables)
                    .Select(@const => new ConstDefinition
                    {
                        LocationType = "Method",
                        LocationName = methodNameWithFullClassName,
                        ConstName = @const.Identifier.ValueText,
                        ConstValue = @const.Initializer?.Value.ToString(),
                    })
            );
        }
    }
}

public sealed record ConstDefinition
{
    public required string LocationType { get; init; }
    public required string? LocationName { get; init; }
    public required string ConstName { get; init; }
    public required string? ConstValue { get; init; }

    public override string ToString()
    {
        return $"[{nameof(LocationType)}] => {LocationType}"
            + $" | [{nameof(LocationName)}] => {LocationName}"
            + $" | [{nameof(ConstName)}] => {ConstName}"
            + $" | [{nameof(ConstValue)}] => {ConstValue}";
    }
}
