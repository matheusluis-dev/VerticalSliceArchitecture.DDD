namespace Application.Domain.Inventories.Entities;

using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class Reservation : IChildEntity
{
    public required ReservationId Id { get; init; }
    public required InventoryId InventoryId { get; init; }
    public required OrderItemId OrderItemId { get; init; }
    public required Quantity Quantity { get; init; }
}
