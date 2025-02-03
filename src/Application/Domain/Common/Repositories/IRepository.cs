namespace Application.Domain.Common.Repositories;

using Application.Domain.Common.Entities;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> GetAll();
    Task<IPagedList<TEntity>> GetPagedAsync(int pageIndex, int pageSize);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
