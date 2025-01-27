namespace Application.Domain.Common.Specifications.Builder;

using System;
using Application.Domain.Common.Entities;

public interface ISpecificationSequence<TEntity, TSpecificationCriteria>
    where TEntity : class, IEntity
    where TSpecificationCriteria : class, ISpecificationCriteria
{
    TSpecificationCriteria And();
    ISpecificationSequence<TEntity, TSpecificationCriteria> And(
        Func<
            ISpecificationCriteria,
            ISpecificationSequence<TEntity, TSpecificationCriteria>
        > criterias
    );

    TSpecificationCriteria Or();
    ISpecificationSequence<TEntity, TSpecificationCriteria> Or(
        Func<
            ISpecificationCriteria,
            ISpecificationSequence<TEntity, TSpecificationCriteria>
        > criterias
    );

    IQueryable<TEntity> GetQueryable();
}
