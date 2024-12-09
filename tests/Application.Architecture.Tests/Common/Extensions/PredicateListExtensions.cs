namespace Application.Architecture.Tests.Common.Extensions;

internal static class PredicateListExtensions
{
    public static string[] GetTypesFullName(this PredicateList predicateList)
    {
        return predicateList.GetTypes().Select(p => p.FullName).ToArray();
    }
}
