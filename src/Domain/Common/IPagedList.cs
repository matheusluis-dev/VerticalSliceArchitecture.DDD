namespace Domain.Common;

public interface IPagedList<TEntity>
    where TEntity : class, IEntity
{
    int PageIndex { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }

    IList<TEntity> Elements { get; }
}
