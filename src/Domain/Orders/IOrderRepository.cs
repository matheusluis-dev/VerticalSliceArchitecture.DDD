namespace Domain.Orders;

using Domain.Common;
using Domain.Orders.Aggregates;
using Domain.Orders.ValueObjects;

public interface IOrderRepository
{
    Task<Result<Order>> FindByIdAsync(OrderId id, CancellationToken ct = default);

    Task<IList<Order>> FindAllPaidOrdersAsync();

    Task<IList<Order>> FindAllPriceOver1000Async();

    Task<IPagedList<Order>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    );

    Task<Order?> CreateAsync(Order order, CancellationToken ct = default);

    void Update(Order order);

    void Delete(Order order);
}
