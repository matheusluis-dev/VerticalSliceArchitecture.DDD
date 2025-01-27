namespace Application.Domain.Common.Repositories;

using Application.Domain.Common.Entities;
using Application.Domain.Common.Specifications.Builder;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> GetAll();
}
