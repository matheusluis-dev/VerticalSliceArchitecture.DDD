namespace Infrastructure.Persistence;

using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal static class VogenExtensions
{
    public static void ApplyVogenEfConvertersFromAssembly(
        this ModelConfigurationBuilder configurationBuilder,
        Assembly assembly
    )
    {
        foreach (var type in assembly.GetTypes())
        {
            if (
                IsVogenValueObject(type)
                && TryGetEfValueConverter(type, out var efCoreConverterType)
            )
            {
                configurationBuilder.Properties(type).HaveConversion(efCoreConverterType);
            }
        }
    }

    private static bool TryGetEfValueConverter(
        Type type,
        [NotNullWhen(true)] out Type? efCoreConverterType
    )
    {
        foreach (var innerType in type.GetNestedTypes())
        {
            if (
                !typeof(ValueConverter).IsAssignableFrom(innerType)
                || !"EfCoreValueConverter".Equals(innerType.Name, StringComparison.Ordinal)
            )
            {
                continue;
            }

            efCoreConverterType = innerType;
            return true;
        }

        efCoreConverterType = null;
        return false;
    }

    private static bool IsVogenValueObject(MemberInfo targetType)
    {
        var generatedCodeAttribute = targetType.GetCustomAttribute<GeneratedCodeAttribute>();

        return "Vogen".Equals(generatedCodeAttribute?.Tool, StringComparison.Ordinal);
    }
}
