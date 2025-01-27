namespace Application.Domain.Common.Specifications.Builder;

using Application.Domain.Common.Entities;

public interface ISpecificationEntryPoint<TEntity, TSpecificationCriteria>
    where TEntity : class, IEntity
    where TSpecificationCriteria : class, ISpecificationCriteria
{
    TSpecificationCriteria SetQueryableCallback(GetQueryableCallback<TEntity> callback);
}
