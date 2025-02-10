namespace Application.Domain.Common.Specifications;

using Application.Domain.Common.Entities;

public interface ISpecification<in TEntity>
    where TEntity : class, IEntity
{
    bool IsSatisfiedBy(TEntity entity);
}
