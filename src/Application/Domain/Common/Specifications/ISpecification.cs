namespace Application.Domain.Common.Specifications;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Application.Domain.Common.Entities;

public interface ISpecification<TEntity>
    where TEntity : class, IEntity
{
    Expression<Func<TEntity, bool>> Criteria { get; }
    IList<Expression<Func<TEntity, object>>> Includes { get; }
}

public interface ISpecificationCriteria;

#pragma warning disable CA1716 // Identifiers should not match keywords
public interface ISpecificationSequence<TEntity>
    where TEntity : class, IEntity
{
    ISpecificationCriteria And();
    ISpecificationSequence<TEntity> And(Func<ISpecificationCriteria, ISpecificationSequence<TEntity>> criterias);

    ISpecificationCriteria Or();
    ISpecificationSequence<TEntity> Or(Func<ISpecificationCriteria, ISpecificationSequence<TEntity>> criterias);

    IQueryable<TEntity> GetQueryable();
}
#pragma warning restore CA1716 // Identifiers should not match keywords
