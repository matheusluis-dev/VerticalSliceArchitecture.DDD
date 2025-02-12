namespace Application.Infrastructure.Persistence.Orders;

using Application.Domain.Orders.Entities;
using Application.Infrastructure.Persistence.Tables;

public sealed class OrderItemMapper : IMapper<OrderItem, OrderItemTable>
{
    public OrderItem ToEntity(OrderItemTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new()
        {
            Id = table.Id,
            OrderId = table.OrderId,
            Quantity = table.Quantity,
            UnitPrice = table.UnitPrice,
            ProductId = table.ProductId,
            ReservationId = table.ReservationId,
        };
    }

    public OrderItemTable ToTable(OrderItem entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            Quantity = entity.Quantity,
            UnitPrice = entity.UnitPrice,
            ProductId = entity.ProductId,
            ReservationId = entity.ReservationId,
        };
    }
}
