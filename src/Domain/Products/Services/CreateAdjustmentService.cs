using Domain.Products.Entities;

namespace Domain.Products.Services;

public sealed record CreateForOrderItemReservationModel(OrderItemId OrderItemId, Reservation Reservation);

public sealed class CreateAdjustmentService
{
    public Result<Adjustment> CreateForOrderItemReservation(CreateForOrderItemReservationModel model)
    {
        var (orderItemId, reservation) = model;

        return Adjustment.Create(
            new AdjustmentId(Guid.NewGuid()),
            reservation.InventoryId,
            orderItemId,
            reservation.Quantity,
            $"Applied adjustment for order item '{orderItemId}'"
        );
    }
}
