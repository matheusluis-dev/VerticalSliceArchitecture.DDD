namespace Application.Domain.Common.Specifications;

using Application.Domain.Common.Entities;

public sealed class OrSpecification<TEntity> : ISpecification<TEntity>
    where TEntity : class, IEntity
{
    private readonly ISpecification<TEntity> _left;
    private readonly ISpecification<TEntity> _right;

    public OrSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
    {
        _left = left;
        _right = right;
    }

    public bool IsSatisfiedBy(TEntity entity)
    {
        return _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
    }
}
