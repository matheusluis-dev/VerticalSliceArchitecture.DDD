namespace Domain.Common.Specifications;

public interface ISpecification<in TEntity>
    where TEntity : class, IEntity
{
    bool IsSatisfiedBy(TEntity entity);
}
