namespace Infrastructure.Persistence.Orders;

using Domain.Orders.Aggregates;
using Infrastructure.Persistence.Tables;

public sealed class OrderMapper : IMapperWithQueryable<Order, OrderTable>
{
    private readonly OrderItemMapper _orderItemMapper;

    public OrderMapper(OrderItemMapper orderItemMapper)
    {
        _orderItemMapper = orderItemMapper;
    }

    public IQueryable<Order> ToEntityQueryable(IQueryable<OrderTable> queryable)
    {
        return queryable.Select(o => ToEntity(o));
    }

    public Order ToEntity(OrderTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var items = table.OrderItems.Select(_orderItemMapper.ToEntity).ToList();
        return new Order
        {
            Id = table.Id,
            Status = table.Status,
            CreatedDate = table.CreatedDate,
            PaidDate = table.PaidDate,
            CustomerEmail = table.CustomerEmail,
            CanceledDate = table.CanceledDate,
            OrderItems = items,
        };
    }

    public OrderTable ToTable(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Status = entity.Status,
            OrderItems = entity.OrderItems.Select(_orderItemMapper.ToTable).ToList(),
            CanceledDate = entity.CanceledDate,
            CustomerEmail = entity.CustomerEmail,
            PaidDate = entity.PaidDate,
            CreatedDate = entity.CreatedDate,
        };
    }
}
