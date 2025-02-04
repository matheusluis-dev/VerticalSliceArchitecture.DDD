namespace Application.Domain.Orders.Repositories;

using Application.Domain.Common;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Specifications.Builder;
using Application.Domain.Orders.ValueObjects;

public interface IOrderRepository
{
    IOrderSpecificationBuilderCriteria Specify { get; }

    Task<Order?> FindByIdAsync(OrderId id, CancellationToken ct = default);

    Task<IPagedList<Order>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    );

    Task<Order?> CreateAsync(Order order, CancellationToken ct = default);

    Task<bool> DeleteAsync(OrderId orderId, CancellationToken ct = default);
}
