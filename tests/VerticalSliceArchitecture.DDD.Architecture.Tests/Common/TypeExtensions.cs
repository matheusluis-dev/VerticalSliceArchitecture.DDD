using System;
using System.Diagnostics.CodeAnalysis;

namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

public static class TypeExtensions
{
    public static bool IsStatic([NotNull] this Type type)
    {
        return type.IsAbstract && type.IsSealed;
    }

    public static bool IsNotStatic([NotNull] this Type type)
    {
        return !type.IsStatic();
    }

    public static bool IsPartial([NotNull] this Type type)
    {
        var partialClasses = PartialClassFinder.GetPartialClassNames();

        return partialClasses.Any(p => p.Equals(type.Name, StringComparison.Ordinal));
    }

    public static bool IsNotPartial([NotNull] this Type type)
    {
        return !type.IsPartial();
    }
}
