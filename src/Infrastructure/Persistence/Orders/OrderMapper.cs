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

        return new OrderBuilder()
            .WithId(table.Id)
            .WithStatus(table.Status)
            .WithCreatedDate(table.CreatedDate)
            .WithPaidDate(table.PaidDate)
            .WithCustomerEmail(table.CustomerEmail)
            .WithCanceledDate(table.CanceledDate)
            .WithOrderItems(items)
            .Build();
    }

    public static OrderTable ToTable(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Status = entity.Status,
            OrderItems = entity.OrderItems.Select(OrderItemMapper.ToTable).ToList(),
            CanceledDate = entity.CancelledDate,
            CustomerEmail = entity.CustomerEmail,
            PaidDate = entity.PaidDate,
            CreatedDate = entity.CreatedDate,
        };
    }
}
