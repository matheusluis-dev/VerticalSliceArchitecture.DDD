namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

using VerticalSliceArchitecture.DDD.Architecture.Tests.Common.Extensions;

internal static class Application
{
    private static readonly Assembly Program = typeof(Program).Assembly;

    internal static IEnumerable<Type> Types => Program.GetTypes();

    internal static IEnumerable<Type> Classes => Types.Where(t => t.IsClass);

    internal static IEnumerable<Type> StaticClasses => Classes.Where(t => t.IsStatic());
    internal static IEnumerable<Type> NonStaticClasses => Classes.Where(t => !t.IsStatic());

    internal static IEnumerable<Type> NonAbstractNonStaticNonPartialClasses =>
        Classes.Where(t => !t.IsAbstract && t.IsNotStatic() && t.IsNotPartial());

    internal static IEnumerable<(Type Class, IEnumerable<PropertyInfo> Properties)> ClassesWithProperties
        => Classes
            .Select(@class => (@class, @class.GetProperties().AsEnumerable()))
            .Where(tuple => tuple.Item2 is not null && tuple.Item2.Any());

    internal static IEnumerable<(Type Class, IEnumerable<MethodInfo> AsyncMethods)> ClassesWithAsyncMethods
        => Classes
            .Select(@class => (@class, @class.GetAsyncMethods()))
            .Where(tuple => tuple.Item2 is not null && tuple.Item2.Any());


    internal static IEnumerable<Type> Interfaces => Types.Where(t => t.IsInterface);
}
