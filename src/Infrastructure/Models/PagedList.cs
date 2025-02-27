using System.Diagnostics.CodeAnalysis;
using Domain.Common.Entities;

namespace Infrastructure.Models;

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

    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
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
