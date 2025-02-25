namespace Domain.Orders.Entities;

using Domain.Common.Entities;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;
using Domain.Orders.ValueObjects;
using Domain.Products.Entities;

public sealed class OrderItem : IChildEntity
{
    public required OrderItemId Id { get; init; }
    public required OrderId OrderId { get; init; }
    public required Product Product { get; init; }
    public ReservationId ReservationId { get; init; }
    public OrderItemPrice OrderItemPrice { get; init; }
}
