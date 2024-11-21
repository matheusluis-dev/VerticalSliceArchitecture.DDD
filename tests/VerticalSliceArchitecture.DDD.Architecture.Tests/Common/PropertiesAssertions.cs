namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public static class PropertiesAssertions
{
    public static IEnumerable<string> PropertiesWithNamesNotPascalCase(this IEnumerable<Type> types)
    {
        ArgumentNullException.ThrowIfNull(types);

        var propertiesWithNameNotPascalCase = new List<string>();

        foreach (var type in types)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (!IsPascalCase(property.Name))
                {
                    propertiesWithNameNotPascalCase.Add($"class '{type.Name}': property '{property.Name}'");
                }
            }
        }

        return propertiesWithNameNotPascalCase;
    }

    private static bool IsPascalCase(string name)
    {
        return char.IsUpper(name.First()) && !name.Contains('_', StringComparison.Ordinal);
    }
}
