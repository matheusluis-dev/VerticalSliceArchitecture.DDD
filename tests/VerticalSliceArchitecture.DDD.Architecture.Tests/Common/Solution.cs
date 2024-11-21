namespace VerticalSliceArchitecture.DDD.Architecture.Tests.Common;

internal static class Application
{
    private static readonly Assembly Program = typeof(Program).Assembly;

    internal static IEnumerable<Type> Types => Program.GetTypes();

    internal static IEnumerable<Type> Classes => Types.Where(t => t.IsClass);
    internal static IEnumerable<Type> Interfaces => Types.Where(t => t.IsInterface);
}
