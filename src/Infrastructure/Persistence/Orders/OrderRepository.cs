namespace Infrastructure.Persistence.Orders;

using System.Linq;
using System.Threading.Tasks;
using Domain.Orders;
using Domain.Orders.Aggregates;
using Domain.Orders.Specifications;
using Domain.Orders.ValueObjects;
using Infrastructure.Persistence.Tables;

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

    public async Task<Result<Order>> FindByIdAsync(OrderId id, CancellationToken ct = default)
    {
        var order = await _orderSet.FindAsync([id], ct);

        if (order is null)
            return Result.NotFound();

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

    public void Update(Order order)
    {
        _orderSet.Update(_mapper.ToTable(order));
    }

    public void Delete(Order order)
    {
        _orderSet.Remove(_mapper.ToTable(order));
    }
}
