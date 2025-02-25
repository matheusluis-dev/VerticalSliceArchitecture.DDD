namespace Infrastructure.Persistence.Orders;

using Domain.Orders.Entities;
using Infrastructure.Persistence.Products;
using Infrastructure.Persistence.Tables;

public static class OrderItemMapper
{
    public static OrderItem ToEntity(OrderItemTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new()
        {
            Id = table.Id,
            OrderId = table.OrderId,
            OrderItemPrice = table.OrderItemPrice,
            Product = ProductMapper.ToEntity(table.Product),
            ReservationId = table.ReservationId,
        };
    }

    public static OrderItemTable ToTable(OrderItem entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            OrderItemPrice = entity.OrderItemPrice,
            Product = ProductMapper.ToTable(entity.Product),
            ReservationId = entity.ReservationId,
        };
    }
}
