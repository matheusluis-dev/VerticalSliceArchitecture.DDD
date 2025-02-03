namespace Application.Domain.Common;

public interface IPagedList<TEntity>
{
    int PageIndex { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }

    Task<IList<TOutput>> GetListAsync<TOutput>(
        Func<IQueryable<TEntity>, IQueryable<TOutput>> expression,
        CancellationToken ct = default
    );
}
