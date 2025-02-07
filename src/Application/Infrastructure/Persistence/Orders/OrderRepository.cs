namespace Application.Infrastructure.Persistence.Orders;

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Domain.Common;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Repositories;
using Application.Domain.Orders.Specifications.Builder;
using Application.Domain.Orders.ValueObjects;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Persistence.Orders.Mappers;
using Application.Infrastructure.Persistence.Orders.Tables;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

public sealed class OrderRepository : IOrderRepository
{
    private readonly DbSet<OrderTable> _orderSet;

    private readonly OrderSpecificationBuilder _specificationBuilder;

    public OrderRepository(
        ApplicationDbContext context,
        OrderSpecificationBuilder specificationBuilder
    )
    {
        Guard.Against.Null(context);

        _orderSet = context.Order;
        _specificationBuilder = specificationBuilder;
    }

    private IQueryable<Order> GetQueryableFromSpecificationBuilder(
        Expression<Func<Order, bool>> predicate
    )
    {
        return GetAll().Where(predicate);
    }

    public IOrderSpecificationBuilderCriteria Specify =>
        _specificationBuilder.SetQueryableCallback(GetQueryableFromSpecificationBuilder);

    public IQueryable<Order> GetAll()
    {
        return _orderSet.AsQueryable().Include(o => o.OrderItems).ToEntityQueryable();
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
            _orderSet
                .AsQueryable()
                .Include(order => order.OrderItems)
                .AsNoTracking()
                .ToEntityQueryable(),
            pageIndex,
            pageSize,
            ct
        );
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
