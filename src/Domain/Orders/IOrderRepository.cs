using Domain.Orders.Aggregates;

namespace Domain.Orders;

public interface IOrderRepository
{
    Task<Result<Order>> FindByIdAsync(OrderId id, CancellationToken ct = default);

    Task<Order?> CreateAsync(Order order, CancellationToken ct = default);

    void Update(Order order);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
