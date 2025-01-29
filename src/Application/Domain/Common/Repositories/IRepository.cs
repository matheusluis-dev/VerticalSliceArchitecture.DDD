namespace Application.Domain.Common.Repositories;

using Application.Domain.Common.Entities;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> GetAll();
    Task AddAsync(TEntity entity);
}
