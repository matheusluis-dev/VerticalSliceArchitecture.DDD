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
