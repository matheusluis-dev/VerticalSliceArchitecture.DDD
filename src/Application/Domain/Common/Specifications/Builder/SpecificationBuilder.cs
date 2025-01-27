namespace Application.Domain.Common.Specifications.Builder;

using System.Linq;
using Application.Domain.Common.Entities;
using Ardalis.GuardClauses;

public abstract class SpecificationBuilder<TEntity, TSpecificationCriteria>
    : ISpecificationBuilder<TEntity, TSpecificationCriteria>
    where TEntity : class, IEntity
    where TSpecificationCriteria : class, ISpecificationCriteria
{
    protected readonly SpecificationBuilderContext<TEntity> _context = new();
    private GetQueryableCallback<TEntity> _callback = null!;

    protected SpecificationBuilder() { }

    //public static ISpecificationEntryPoint<TEntity, TSpecificationCriteria> Create()
    //{
    //    return new SpecificationBuilder<TEntity, TSpecificationCriteria>();
    //}

    public TSpecificationCriteria SetQueryableCallback(GetQueryableCallback<TEntity> callback)
    {
        if (_callback is not null)
            throw new InvalidOperationException($"{nameof(_callback)} already initialized");

        _callback = callback;
        return this as TSpecificationCriteria;
    }

    public TSpecificationCriteria And()
    {
        return this as TSpecificationCriteria;
    }

    public ISpecificationSequence<TEntity, TSpecificationCriteria> And(
        Func<
            ISpecificationCriteria,
            ISpecificationSequence<TEntity, TSpecificationCriteria>
        > criterias
    )
    {
        Guard.Against.Null(criterias);

        criterias(this);
        return this;
    }

    public TSpecificationCriteria Or()
    {
        _context.Or();
        return this as TSpecificationCriteria;
    }

    public ISpecificationSequence<TEntity, TSpecificationCriteria> Or(
        Func<
            ISpecificationCriteria,
            ISpecificationSequence<TEntity, TSpecificationCriteria>
        > criterias
    )
    {
        Guard.Against.Null(criterias);

        _context.Or();
        criterias(this);

        return this;
    }

    public ISpecificationSequence<TEntity, TSpecificationCriteria> Include(
        ISpecification<TEntity> specification
    )
    {
        _context.AddSpecification(specification);
        return this;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        if (_callback is null)
            throw new InvalidOperationException($"{nameof(_callback)} was not set");

        var predicate = _context.GetPredicate();

        return _callback(predicate);
    }
}
