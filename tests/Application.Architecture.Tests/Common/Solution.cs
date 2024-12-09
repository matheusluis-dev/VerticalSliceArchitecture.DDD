namespace Application.Architecture.Tests.Common;

using System.Reflection;

internal static class Solution
{
    private static readonly Assembly Application = typeof(Program).Assembly;

    internal static Types AllTypes => Types.InAssembly(Application);

    internal static PredicateList TypesFromApplication =>
        AllTypes.That().ResideInNamespace(Namespaces.Layer.Application);

    internal static PredicateList DomainTypes =>
        AllTypes.That().ResideInNamespace(Namespaces.Layer.Domain);
    internal static PredicateList NonDomainTypes =>
        AllTypes.That().DoNotResideInNamespace(Namespaces.Layer.Domain);

    internal static PredicateList InfrastructureTypes =>
        AllTypes.That().ResideInNamespace(Namespaces.Layer.Infrastructure);
    internal static PredicateList NonInfrastructureTypes =>
        AllTypes.That().DoNotResideInNamespace(Namespaces.Layer.Infrastructure);

    internal static PredicateList NonDomainNonInfrastructureTypes =>
        AllTypes
            .That()
            .DoNotResideInNamespace(Namespaces.Layer.Domain)
            .And()
            .DoNotResideInNamespace(Namespaces.Layer.Infrastructure);
}
