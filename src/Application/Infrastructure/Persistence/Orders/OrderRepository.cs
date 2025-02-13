namespace Application.Infrastructure.Persistence.Orders;

using System.Linq;
using System.Threading.Tasks;
using Application.Domain.__Common;
using Application.Domain.Orders;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Specifications;
using Application.Domain.Orders.ValueObjects;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class OrderRepository : IOrderRepository
{
    private readonly DbSet<OrderTable> _orderSet;
    private readonly OrderMapper _mapper;

    public OrderRepository(ApplicationDbContext context, OrderMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);

        _orderSet = context.Order;
        _mapper = mapper;
    }

    private IQueryable<OrderTable> GetDefaultQuery()
    {
        return _orderSet.AsQueryable().Include(o => o.OrderItems);
    }

    public IQueryable<Order> GetAll()
    {
        return _mapper.ToEntityQueryable(GetDefaultQuery());
    }

    public async Task<Order?> FindByIdAsync(OrderId id, CancellationToken ct = default)
    {
        var order = await _orderSet.FindAsync([id], ct);

        if (order is null)
            return null;

        return _mapper.ToEntity(order);
    }

    public async Task<IPagedList<Order>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    )
    {
        return await PagedList<Order>.CreateAsync(
            _mapper.ToEntityQueryable(GetDefaultQuery().AsNoTracking()),
            pageIndex,
            pageSize,
            ct
        );
    }

    public async Task<IList<Order>> FindAllPaidOrdersAsync()
    {
        return await _mapper
            .ToEntityQueryable(GetDefaultQuery().AsNoTracking())
            .Where(order => new ArePaidSpecification().IsSatisfiedBy(order))
            .ToListAsync();
    }

    public async Task<IList<Order>> FindAllPriceOver1000Async()
    {
        return await _mapper
            .ToEntityQueryable(GetDefaultQuery())
            .AsNoTracking()
            .Where(order => new TotalPriceHigherThan1000Specification().IsSatisfiedBy(order))
            .ToListAsync();
    }

    public async Task<Order?> CreateAsync(Order order, CancellationToken ct = default)
    {
        await _orderSet.AddAsync(_mapper.ToTable(order), ct);

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
