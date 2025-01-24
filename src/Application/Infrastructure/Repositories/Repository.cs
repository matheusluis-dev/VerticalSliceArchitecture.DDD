namespace Application.Infrastructure.Repositories;

using System.Linq.Expressions;
using Application.Domain.Common.Entities;
using Application.Domain.Common.Repositories;
using Application.Domain.Common.Specifications;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

public class Repository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _set;

    public Repository(TContext context)
    {
        _context = context;
        _set = _context.Set<TEntity>();
    }

    public IEnumerable<TEntity> Specify(ISpecification<TEntity> specification)
    {
        Guard.Against.Null(specification);

        var includes = specification.Includes.Aggregate(
            _context.Set<TEntity>().AsQueryable(),
            (current, include) => current.Include(include)
        );

        return includes.Where(specification.Criteria).AsEnumerable();
    }



    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await _set.AddAsync(entity, ct);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken ct = default
    )
    {
        await _set.AddRangeAsync(entities, ct);
        return entities;
    }

    public void Delete(TEntity entity)
    {
        _set.Remove(entity);
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default
    )
    {
        return await _set.FirstOrDefaultAsync(predicate, ct);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _set.ToListAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _set.FindAsync(id, ct);
    }

    public void Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }
}
