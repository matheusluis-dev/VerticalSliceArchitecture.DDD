namespace Application.Infrastructure;

using Application.Domain.Common;
using Microsoft.EntityFrameworkCore;

public class PagedList<T> : IPagedList<T>
{
    public int PageIndex { get; }
    public int TotalPages { get; }

    public PagedList(IQueryable<T> queryable, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        Queryable = queryable;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public IQueryable<T> Queryable { get; init; }

    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageIndex,
        int pageSize
    )
    {
        var count = await source.CountAsync();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return new PagedList<T>(items, count, pageIndex, pageSize);
    }
}
