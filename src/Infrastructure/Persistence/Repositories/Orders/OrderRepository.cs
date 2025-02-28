using Domain.Orders;
using Domain.Orders.Aggregates;
using Domain.Orders.Ids;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Orders;

public sealed class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<OrderTable> _set;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
        _set = _context.Set<OrderTable>();
    }

    public async Task<Result<Order>> FindByIdAsync(OrderId id, CancellationToken ct = default)
    {
        var order = await _set.FindAsync([id], ct);

        if (order is null)
            return Result.Failure();

        return OrderMapper.ToEntity(order);
    }

    public async Task<Order?> CreateAsync(Order order, CancellationToken ct = default)
    {
        await _set.AddAsync(OrderMapper.ToTable(order), ct);

        return order;
    }

    public void Update(Order order)
    {
        _set.Update(OrderMapper.ToTable(order));
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}
