namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public static class PropertiesAssertions
{
    public static IEnumerable<string> PropertiesWithNamesNotPascalCase(this IEnumerable<IType> types)
    {
        foreach (var type in types)
        {
            var properties = type.ReflectionType.GetProperties();
            foreach (var property in properties)
            {
                if (!IsPascalCase(property.Name))
                    yield return $"class '{type.Name}': property '{property.Name}'";
            }
        }
    }

    private static bool IsPascalCase(string name)
    {
        return char.IsUpper(name.First()) && !name.Contains('_');
    }
}
