namespace Infrastructure.Persistence.Orders;

using Domain.Orders.Aggregates;
using Infrastructure.Persistence.Tables;

public static class OrderMapper
{
    public static IQueryable<Order> ToEntityQueryable(IQueryable<OrderTable> queryable)
    {
        return queryable.Select(o => ToEntity(o));
    }

    public static Order ToEntity(OrderTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var items = table.OrderItems.Select(OrderItemMapper.ToEntity).ToList();
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

    public static OrderTable ToTable(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Status = entity.Status,
            OrderItems = entity.OrderItems.Select(OrderItemMapper.ToTable).ToList(),
            CanceledDate = entity.CanceledDate,
            CustomerEmail = entity.CustomerEmail,
            PaidDate = entity.PaidDate,
            CreatedDate = entity.CreatedDate,
        };
    }
}
