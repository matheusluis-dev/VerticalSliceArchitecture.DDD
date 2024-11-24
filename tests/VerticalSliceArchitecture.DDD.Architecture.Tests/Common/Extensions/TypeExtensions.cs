namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Extensions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

internal static class TypeExtensions
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

    public static IEnumerable<MethodInfo> GetAsyncMethods([NotNull] this Type type)
    {
        foreach (var method in type.GetMethods())
        {
            var asyncStateMachineAttribute = method.GetCustomAttribute<AsyncStateMachineAttribute>();
            var isAsync = asyncStateMachineAttribute is not null;

            if (isAsync)
            {
                yield return method;
            }
        }
    }

    public static IEnumerable<MethodInfo> GetStaticMethods([NotNull] this Type type)
    {
        return type.GetMethods().Where(method => method.IsStatic);
    }
}
