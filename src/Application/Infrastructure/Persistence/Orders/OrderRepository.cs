namespace Application.Infrastructure.Persistence.Orders;

using System.Linq;
using System.Threading.Tasks;
using Application.Domain.Common;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Repositories;
using Application.Domain.Orders.Specifications;
using Application.Domain.Orders.ValueObjects;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Persistence.Orders.Mappers;
using Application.Infrastructure.Persistence.Orders.Tables;
using Microsoft.EntityFrameworkCore;

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
        return GetDefaultQuery().ToEntityQueryable();
    }

    public async Task<Order?> FindByIdAsync(OrderId id, CancellationToken ct = default)
    {
        var order = await _orderSet.FindAsync([id], ct);

        if (order is null)
            return null;

        return order.ToEntity();
    }

    public async Task<IPagedList<Order>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    )
    {
        return await PagedList<Order>.CreateAsync(
            GetDefaultQuery().AsNoTracking().ToEntityQueryable(),
            pageIndex,
            pageSize,
            ct
        );
    }

    public async Task<IList<Order>> FindAllPaidOrdersAsync()
    {
        return await GetDefaultQuery()
            .AsNoTracking()
            .ToEntityQueryable()
            .Where(order => new ArePaidSpecification().IsSatisfiedBy(order))
            .ToListAsync();
    }

    public async Task<IList<Order>> FindAllPriceOver1000Async()
    {
        return await GetDefaultQuery()
            .AsNoTracking()
            .ToEntityQueryable()
            .Where(order => new TotalPriceHigherThan1000Specification().IsSatisfiedBy(order))
            .ToListAsync();
    }

    public async Task<Order?> CreateAsync(Order order, CancellationToken ct = default)
    {
        await _orderSet.AddAsync(order.FromEntity(), ct);

        return order;
    }

    public async Task<bool> DeleteAsync(OrderId orderId, CancellationToken ct = default)
    {
        var order = await _orderSet.FirstOrDefaultAsync(order => order.Id == orderId, ct);

        if (order is null)
            return false;

        _orderSet.Remove(order);

        return true;
    }
}
