namespace Infrastructure.Persistence.Orders;

using System.Linq;
using System.Threading.Tasks;
using Domain.Orders;
using Domain.Orders.Aggregates;
using Domain.Orders.Enums;
using Domain.Orders.ValueObjects;
using Infrastructure.Persistence.Tables;

public sealed class OrderRepository : IOrderRepository
{
    private readonly DbSet<OrderTable> _orderSet;

    public OrderRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _orderSet = context.Order;
    }

    private IQueryable<OrderTable> GetDefaultQuery()
    {
        return _orderSet.AsQueryable().Include(o => o.OrderItems);
    }

    public IQueryable<Order> GetAll()
    {
        return OrderMapper.ToEntityQueryable(GetDefaultQuery());
    }

    public async Task<Result<Order>> FindByIdAsync(OrderId id, CancellationToken ct = default)
    {
        var order = await _orderSet.FindAsync([id], ct);

        if (order is null)
            return Result.NotFound();

        return OrderMapper.ToEntity(order);
    }

    public async Task<IPagedList<Order>> FindAllPagedAsync(int pageIndex, int pageSize, CancellationToken ct = default)
    {
        return await PagedList<Order>.CreateAsync(
            OrderMapper.ToEntityQueryable(GetDefaultQuery().AsNoTracking()),
            pageIndex,
            pageSize,
            ct
        );
    }

    public async Task<IList<Order>> FindAllPaidOrdersAsync()
    {
        return await OrderMapper
            .ToEntityQueryable(GetDefaultQuery().AsNoTracking())
            .Where(order => order.Status == OrderStatus.Paid)
            .ToListAsync();
    }

    public async Task<Order?> CreateAsync(Order order, CancellationToken ct = default)
    {
        await _orderSet.AddAsync(OrderMapper.ToTable(order), ct);

        return order;
    }

    public void Update(Order order)
    {
        _orderSet.Update(OrderMapper.ToTable(order));
    }

    public void Delete(Order order)
    {
        _orderSet.Remove(OrderMapper.ToTable(order));
    }
}
