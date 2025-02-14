namespace Domain.Common.Specifications;

using Domain.Common.Entities;

public interface ISpecification<in TEntity>
    where TEntity : class, IEntity
{
    bool IsSatisfiedBy(TEntity entity);
}
