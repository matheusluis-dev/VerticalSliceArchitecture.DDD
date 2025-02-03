namespace Application.Domain.Common;
public interface IPagedList<T>
{
    int PageIndex { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
    IQueryable<T> Queryable { get; }
}
