namespace Application.Architecture.Tests.Common.Extensions;

public static class StringExtensions
{
    public static bool IsPascalCased(this string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);

        return char.IsUpper(str[0]) && str.All(char.IsLetterOrDigit);
    }

    public static bool IsNotPascalCased(this string str)
    {
        return !str.IsPascalCased();
    }
}
