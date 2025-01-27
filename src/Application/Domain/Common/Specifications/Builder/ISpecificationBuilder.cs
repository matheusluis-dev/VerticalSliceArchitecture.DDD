namespace Application.Domain.Common.Specifications.Builder;

using Application.Domain.Common.Entities;

public interface ISpecificationBuilder<TEntity, TSpecificationCriteria>
    : ISpecificationEntryPoint<TEntity, TSpecificationCriteria>,
        ISpecificationSequence<TEntity, TSpecificationCriteria>,
        ISpecificationCriteria
    where TEntity : class, IEntity
    where TSpecificationCriteria : class, ISpecificationCriteria;
