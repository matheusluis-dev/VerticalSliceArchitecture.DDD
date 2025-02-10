namespace Application.Domain.Orders.Repositories;

using Application.Domain.Common;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.ValueObjects;

public interface IOrderRepository
{
    Task<Order?> FindByIdAsync(OrderId id, CancellationToken ct = default);

    Task<IList<Order>> FindAllPaidOrdersAsync();

    Task<IList<Order>> FindAllPriceOver1000Async();

    Task<IPagedList<Order>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    );

    Task<Order?> CreateAsync(Order order, CancellationToken ct = default);

    Task<bool> DeleteAsync(OrderId orderId, CancellationToken ct = default);
}
