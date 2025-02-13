namespace Application.Domain.Orders.Entities;

using Application.Domain.__Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed class OrderItem : IChildEntity
{
    public required OrderItemId Id { get; init; }
    public required OrderId OrderId { get; init; }
    public required ProductId ProductId { get; init; }
    public ReservationId ReservationId { get; init; }
    public Quantity Quantity { get; init; }
    public Amount UnitPrice { get; init; }
}
