namespace Application.Domain.Common.Specifications.Builder;

using System.Linq.Expressions;
using Application.Domain.Common.Entities;

public delegate IQueryable<TEntity> GetQueryableCallback<TEntity>(
    Expression<Func<TEntity, bool>> predicate
)
    where TEntity : class, IEntity;
