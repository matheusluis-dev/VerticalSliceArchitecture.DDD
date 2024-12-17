namespace Application.Architecture.Tests.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

internal static class EnumerableStringExtensions
{
    public static string ToUnorderedStringList(
        this IEnumerable<string> enumerable,
        char marker,
        int? tabWidth = null
    )
    {
        if (!enumerable.Any())
            return string.Empty;

        var tab = new string(' ', tabWidth.GetValueOrDefault());

        return string.Join(Environment.NewLine, enumerable!.Select(e => $"{tab}{marker} {e}"));
    }
}
