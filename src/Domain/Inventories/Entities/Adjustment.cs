namespace Domain.Inventories.Entities;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;

public sealed class Adjustment : IChildEntity
{
    public AdjustmentId Id { get; init; }
    public InventoryId InventoryId { get; init; }
    public OrderItemId? OrderItemId { get; init; }
    public Quantity Quantity { get; init; }
    public string Reason { get; init; }

    internal static Adjustment CreateForOrderItemReservation(
        OrderItemId orderItemId,
        Reservation reservation
    )
    {
        return new Adjustment
        {
            Id = AdjustmentId.Create(),
            InventoryId = reservation.InventoryId,
            OrderItemId = orderItemId,
            Quantity = reservation.Quantity,
            Reason = $"Applied adjustment for order item '{orderItemId}'",
        };
    }
}
