using Domain.Orders.Aggregates;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Orders;

internal static class OrderMapper
{
    internal static IQueryable<Order> ToEntityQueryable(IQueryable<OrderTable> queryable)
    {
        return queryable.Select(o => ToEntity(o));
    }

    internal static Order ToEntity(OrderTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var items = table.OrderItems.Select(OrderItemMapper.ToEntity).ToList();

        return OrderBuilder
            .Create()
            .WithId(table.Id)
            .WithStatus(table.Status)
            .WithCreatedDate(table.CreatedDate)
            .WithPaidDate(table.PaidDate)
            .WithCustomerEmail(table.CustomerEmail)
            .WithCanceledDate(table.CanceledDate)
            .WithOrderItems(items)
            .Build();
    }

    internal static OrderTable ToTable(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new OrderTable
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
