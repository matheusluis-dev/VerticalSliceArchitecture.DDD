namespace Application.Domain.Common.Specifications;

using System.Linq.Expressions;
using Application.Domain.Common.Entities;

public abstract class SpecificationBase<TEntity> : ISpecification<TEntity>
    where TEntity : class, IEntity
{
    protected SpecificationBase(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>> Criteria { get; }

    public IList<Expression<Func<TEntity, object>>> Includes { get; } = [];

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}
