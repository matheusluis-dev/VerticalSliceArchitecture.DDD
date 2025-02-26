using Domain.Orders.Aggregates;

namespace Domain.Orders;

public interface IOrderRepository
{
    [return: NotNull]
    Task<Result<Order>> FindByIdAsync(OrderId id, CancellationToken ct = default);

    Task<IList<Order>> FindAllPaidOrdersAsync();

    Task<IPagedList<Order>> FindAllPagedAsync(int pageIndex, int pageSize, CancellationToken ct = default);

    [return: NotNull]
    Task<Order?> CreateAsync(Order order, CancellationToken ct = default);

    void Update(Order order);

    void Delete(Order order);
}
