namespace Application.Domain.Common.Repositories;

using Application.Domain.Common.Entities;

public interface IRepository<out TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> GetAll();
}
