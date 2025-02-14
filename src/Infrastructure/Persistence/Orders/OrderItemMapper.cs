namespace Infrastructure.Persistence.Orders;

using Domain.Orders.Entities;
using Infrastructure.Persistence.Products;
using Infrastructure.Persistence.Tables;

public sealed class OrderItemMapper : IMapper<OrderItem, OrderItemTable>
{
    private readonly ProductMapper _productMapper;

    public OrderItemMapper(ProductMapper productMapper)
    {
        _productMapper = productMapper;
    }

    public OrderItem ToEntity(OrderItemTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new()
        {
            Id = table.Id,
            OrderId = table.OrderId,
            Quantity = table.Quantity,
            UnitPrice = table.UnitPrice,
            Product = _productMapper.ToEntity(table.Product),
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
            Product = _productMapper.ToTable(entity.Product),
            ReservationId = entity.ReservationId,
        };
    }
}
