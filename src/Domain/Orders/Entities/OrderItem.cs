using Domain.Orders.ValueObjects;
using Domain.Products.Entities;

namespace Domain.Orders.Entities;

public sealed class OrderItem : IChildEntity
{
    public OrderItemId Id { get; init; }
    public OrderId OrderId { get; init; }
    public Product Product { get; init; }
    public ReservationId ReservationId { get; init; }
    public OrderItemPrice OrderItemPrice { get; init; }

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
