namespace Application.Architecture.Tests.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

internal static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable?.Any() != true;
    }

    public static string ToUnorderedStringList(
        this IEnumerable<string>? enumerable,
        char marker,
        int? tabWidth = null
    )
    {
        if (enumerable.IsNullOrEmpty())
            return string.Empty;

        var tab = tabWidth is null ? string.Empty : new string(' ', tabWidth.Value);

        return string.Join(Environment.NewLine, enumerable!.Select(e => $"{tab}{marker} {e}"));
    }
}
