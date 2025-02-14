namespace Infrastructure.Persistence;

public interface IMapper<TEntity, TTable>
    where TEntity : class
    where TTable : class
{
    TEntity ToEntity(TTable table);
    TTable ToTable(TEntity entity);
}
