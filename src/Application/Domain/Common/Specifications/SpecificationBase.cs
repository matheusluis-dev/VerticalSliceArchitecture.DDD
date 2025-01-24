namespace Application.Domain.Common.Specifications;

using System.Linq;
using System.Linq.Expressions;
using Application.Domain.Common.Entities;
using Ardalis.GuardClauses;

public abstract class SpecificationBase<TEntity>
    : ISpecification<TEntity>,
        ISpecificationCriteria,
        ISpecificationSequence<TEntity>
    where TEntity : class, IEntity
{
    private readonly SpecificationContext<TEntity> _context = new();

    protected SpecificationBase(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>> Criteria { get; }

    public IList<Expression<Func<TEntity, object>>> Includes { get; } = [];

    public ISpecificationCriteria And()
    {
        return this;
    }

    public ISpecificationSequence<TEntity> And(
        Func<ISpecificationCriteria, ISpecificationSequence<TEntity>> criterias
    )
    {
        Guard.Against.Null(criterias);

        criterias(this);
        return this;
    }

    public ISpecificationCriteria Or()
    {
        _context.Or();
        return this;
    }

    public ISpecificationSequence<TEntity> Or(
        Func<ISpecificationCriteria, ISpecificationSequence<TEntity>> criterias
    )
    {
        Guard.Against.Null(criterias);

        _context.Or();
        criterias(this);

        return this;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _context.GetQueryable();
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}
