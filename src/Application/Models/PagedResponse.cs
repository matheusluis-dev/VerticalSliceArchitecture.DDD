using Domain.Common.Entities;

namespace Application.Models;

public abstract record PagedResponse<TResponse, TEntity>
    where TResponse : class
    where TEntity : class, IEntity
{
    public IList<TResponse> Elements { get; init; }

    public int PageIndex { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }

    protected PagedResponse(IPagedList<TEntity> pagedList, Func<TEntity, TResponse> mapper)
    {
        ArgumentNullException.ThrowIfNull(pagedList);
        ArgumentNullException.ThrowIfNull(mapper);

        PageIndex = pagedList.PageIndex;
        TotalPages = pagedList.TotalPages;
        HasPreviousPage = pagedList.HasPreviousPage;
        HasNextPage = pagedList.HasNextPage;
        Elements = pagedList.Elements.Select(mapper).ToList();
    }
}
