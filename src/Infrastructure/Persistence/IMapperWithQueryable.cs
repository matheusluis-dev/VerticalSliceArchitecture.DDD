namespace Infrastructure.Persistence;

public interface IMapperWithQueryable<TEntity, TTable> : IMapper<TEntity, TTable>
    where TEntity : class
    where TTable : class
{
    IQueryable<TEntity> ToEntityQueryable(IQueryable<TTable> queryable);
}
