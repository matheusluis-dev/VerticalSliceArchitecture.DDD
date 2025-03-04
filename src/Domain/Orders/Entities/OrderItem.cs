using Domain.Orders.ValueObjects;

namespace Domain.Orders.Entities;

public sealed class OrderItem : IChildEntity
{
    public OrderItemId Id { get; init; }
    public OrderId OrderId { get; init; }
    public ProductId ProductId { get; init; }
    public ReservationId ReservationId { get; init; }
    public OrderItemPrice OrderItemPrice { get; init; }

    public OrderItem(
        OrderItemId id,
        OrderId orderId,
        ProductId productId,
        ReservationId? reservationId,
        OrderItemPrice orderItemPrice
    )
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        ReservationId = reservationId!;
        OrderItemPrice = orderItemPrice;
    }
}
