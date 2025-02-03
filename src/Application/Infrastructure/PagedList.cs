namespace Application.Infrastructure;

using Application.Domain.Common;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

public class PagedList<T> : IPagedList<T>
{
    public int PageIndex { get; }
    public int TotalPages { get; }

    private readonly IQueryable<T> _queryable;
    private readonly int _count;
    private readonly int _pageSize;

    public PagedList(IQueryable<T> queryable, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        _count = count;
        _queryable = queryable;
        _pageSize = pageSize;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public async Task<IList<TOutput>> GetListAsync<TOutput>(
        Func<IQueryable<T>, IQueryable<TOutput>> expression,
        CancellationToken ct = default
    )
    {
        Guard.Against.Null(expression);

        return await expression(_queryable)
            .Skip((PageIndex - 1) * _pageSize)
            .Take(_pageSize)
            .ToListAsync(ct);
    }

    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageIndex,
        int pageSize
    )
    {
        var count = await source.CountAsync();

        return new PagedList<T>(source, count, pageIndex, pageSize);
    }
}
