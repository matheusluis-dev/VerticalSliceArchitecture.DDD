namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Extensions;

using System.Diagnostics.CodeAnalysis;

internal static class StringExtensions
{
    internal static bool IsPascalCase([NotNull] this string str)
    {
        return char.IsUpper(str.First()) && !str.Contains('_', StringComparison.Ordinal);
    }
}
