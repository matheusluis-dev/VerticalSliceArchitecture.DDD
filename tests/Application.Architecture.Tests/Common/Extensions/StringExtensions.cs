namespace Application.Architecture.Tests.Common.Extensions;

public static class StringExtensions
{
    public static bool IsPascalCased(this string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);

        return char.IsUpper(str[0]) && str.All(char.IsLetter);
    }

    public static bool IsNotPascalCased(this string str)
    {
        return !str.IsPascalCased();
    }

    public static bool IsUnderscoreCamelCased(this string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);

        return str.Length >= 2
            && str[0].Equals('_')
            && char.IsLower(str[1])
            && str[1..].All(char.IsLetter);
    }

    public static bool IsNotUnderscoreCamelCased(this string str)
    {
        return !str.IsUnderscoreCamelCased();
    }

    public static bool IsUpperCased(this string str)
    {
        return str.All(char.IsUpper);
    }

    public static bool IsNotUpperCased(this string str)
    {
        return !str.IsUpperCased();
    }
}
