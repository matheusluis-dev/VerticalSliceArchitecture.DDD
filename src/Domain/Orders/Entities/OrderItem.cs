using Domain.Orders.ValueObjects;
using Domain.Products.Entities;

namespace Domain.Orders.Entities;

public sealed class OrderItem : IChildEntity
{
    public OrderItemId Id { get; init; } = null!;
    public OrderId OrderId { get; init; } = null!;
    public Product Product { get; init; } = null!;
    public ReservationId ReservationId { get; init; } = null!;
    public OrderItemPrice OrderItemPrice { get; init; } = null!;

    private OrderItem() { }

    public OrderItem(
        OrderItemId id,
        OrderId orderId,
        Product product,
        ReservationId? reservationId,
        OrderItemPrice orderItemPrice
    )
    {
        Id = id;
        OrderId = orderId;
        Product = product;
        ReservationId = reservationId!;
        OrderItemPrice = orderItemPrice;
    }
}
