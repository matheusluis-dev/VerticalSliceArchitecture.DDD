namespace Application.Architecture.Tests.Common;

using System.Reflection;
using FastEndpoints;

/// <summary>
/// System Under Test
/// </summary>
internal static class Sut
{
    private static readonly Assembly _applicationAssembly = typeof(Program).Assembly;

    private static NetArchTest.Rules.Types TypesFromApplicationAssembly =>
        NetArchTest.Rules.Types.InAssembly(_applicationAssembly);

    private static PredicateList WithoutFastEndpointsSelfGeneratedTypes(
        this NetArchTest.Rules.Types predicate
    )
    {
        return predicate.That().DoNotHaveName([.. Constants.FastEndpointsSelfGeneratedFiles]);
    }

    internal static PredicateList Types =>
        TypesFromApplicationAssembly.WithoutFastEndpointsSelfGeneratedTypes();

    internal static PredicateList Interfaces => Types.That().AreInterfaces();

    internal static PredicateList Classes => Types.That().AreClasses();

    internal static PredicateList Structs => Types.That().AreStructures();

    internal static PredicateList Enums => Types.That().AreEnums();

    internal static Predicate That(this PredicateList predicate)
    {
        return predicate.And();
    }

    internal static PredicateList WithDomainTypesOnly(this PredicateList predicate)
    {
        return predicate.That().ResideInNamespace(Namespaces.Layer.Domain);
    }

    internal static PredicateList WithNonDomainTypesOnly(this PredicateList predicate)
    {
        return predicate.That().DoNotResideInNamespace(Namespaces.Layer.Domain);
    }

    internal static PredicateList WithInfrastructureTypesOnly(this PredicateList predicate)
    {
        return predicate.That().ResideInNamespace(Namespaces.Layer.Infrastructure);
    }

    internal static PredicateList WithNonInfrastructureTypesOnly(this PredicateList predicate)
    {
        return predicate.That().DoNotResideInNamespace(Namespaces.Layer.Infrastructure);
    }

    internal static PredicateList WithApplicationLayerTypesOnly(this PredicateList predicate)
    {
        return predicate.WithNonDomainTypesOnly().WithNonInfrastructureTypesOnly();
    }

    internal static PredicateList WithEndpointsOnly(this PredicateList predicate)
    {
        return predicate
            .That()
            .Inherit(typeof(Endpoint<>))
            .Or()
            .Inherit(typeof(Endpoint<,>))
            .Or()
            .Inherit(typeof(Endpoint<,,>))
            .Or()
            .Inherit<EndpointWithoutRequest>()
            .Or()
            .Inherit(typeof(EndpointWithoutRequest<>))
            .Or()
            .Inherit(typeof(EndpointWithoutRequest<,>));
    }

    internal static PredicateList WithNonEndpointsOnly(this PredicateList predicate)
    {
        return predicate
            .That()
            .DoNotInherit(typeof(Endpoint<>))
            .And()
            .DoNotInherit(typeof(Endpoint<,>))
            .And()
            .DoNotInherit(typeof(Endpoint<,,>))
            .And()
            .DoNotInherit<EndpointWithoutRequest>()
            .And()
            .DoNotInherit(typeof(EndpointWithoutRequest<>))
            .And()
            .DoNotInherit(typeof(EndpointWithoutRequest<,>));
    }
}
