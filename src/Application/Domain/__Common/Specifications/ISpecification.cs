namespace Application.Domain.__Common.Specifications;

using Application.Domain.__Common.Entities;

public interface ISpecification<in TEntity>
    where TEntity : class, IEntity
{
    bool IsSatisfiedBy(TEntity entity);
}
