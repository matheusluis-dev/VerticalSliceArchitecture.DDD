namespace Domain.Inventories.Entities;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;

public sealed class Reservation : IChildEntity
{
    public required ReservationId Id { get; init; }
    public required InventoryId InventoryId { get; init; }
    public required OrderItemId OrderItemId { get; init; }
    public required Quantity Quantity { get; init; }
}
