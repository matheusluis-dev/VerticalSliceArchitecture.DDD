using Domain.Inventories.Aggregate;
using Domain.Inventories.Entities;

namespace Domain.Inventories.Services;

public sealed record CreateForOrderItemReservationModel(
    Inventory Inventory,
    OrderItemId OrderItemId,
    Reservation Reservation
);

public sealed class CreateAdjustmentService
{
    public Result<Adjustment> CreateForOrderItemReservation(CreateForOrderItemReservationModel model)
    {
        var (inventory, orderItemId, reservation) = model;

        // TODO: only 1 reservation per order item

        return Adjustment.Create(
            new AdjustmentId(Guid.NewGuid()),
            reservation.InventoryId,
            orderItemId,
            reservation.Quantity,
            $"Applied adjustment for order item '{orderItemId}'"
        );
    }
}
