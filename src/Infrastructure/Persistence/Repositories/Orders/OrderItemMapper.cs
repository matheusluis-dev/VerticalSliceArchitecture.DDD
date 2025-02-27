using Domain.Orders.Entities;
using Infrastructure.Persistence.Repositories.Products;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Orders;

internal static class OrderItemMapper
{
    internal static OrderItem ToEntity(OrderItemTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new OrderItem(
            table.Id,
            table.OrderId,
            ProductMapper.ToEntity(table.Product!),
            table.ReservationId,
            table.OrderItemPrice
        );
    }

    internal static OrderItemTable ToTable(OrderItem entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new OrderItemTable
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            OrderItemPrice = entity.OrderItemPrice,
            ProductId = entity.Product.Id,
            Product = ProductMapper.ToTable(entity.Product),
            ReservationId = entity.ReservationId,
        };
    }
}
