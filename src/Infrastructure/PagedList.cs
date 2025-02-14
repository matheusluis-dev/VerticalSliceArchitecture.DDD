namespace Infrastructure;

using Domain.Common;
using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class PagedList<TEntity> : IPagedList<TEntity>
    where TEntity : class, IEntity
{
    public int PageIndex { get; }

    public int TotalPages { get; }

    public IList<TEntity> Elements { get; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    private PagedList(IList<TEntity> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        Elements = items;
    }

    public static async Task<PagedList<TEntity>> CreateAsync(
        IQueryable<TEntity> source,
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var count = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<TEntity>(items, count, pageIndex, pageSize);
    }
}
