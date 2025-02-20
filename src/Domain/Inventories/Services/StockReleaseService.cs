namespace Domain.Inventories.Services;

using Domain.Inventories.Aggregate;
using Domain.Inventories.Entities;
using Domain.Inventories.Enums;
using Domain.Orders.ValueObjects;

public sealed record ReleaseStockReservationModel(Inventory Inventory, OrderItemId OrderItemId)
{
    public CreateForOrderItemReservationModel ToCreateForOrderItemReservationModel(Reservation reservation)
    {
        return new(Inventory, OrderItemId, reservation);
    }
}

public sealed class StockReleaseService
{
    private readonly CreateAdjustmentService _createAdjustment;

    public StockReleaseService(CreateAdjustmentService createAdjustment)
    {
        _createAdjustment = createAdjustment;
    }

    public Result<Inventory> ReleaseStockReservation(ReleaseStockReservationModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var (inventory, orderItemId) = model;

        var reservation = inventory.Reservations.FirstOrDefault(r => r.OrderItemId == orderItemId);

        if (reservation is null)
            return Result.Invalid(new ValidationError($"Reservation not found for order item '{orderItemId}'"));

        if (reservation.Status is ReservationStatus.Cancelled)
            return Result.Invalid(new ValidationError("Can not apply if reservation is cancelled"));

        if (reservation.Status is ReservationStatus.Applied)
            return Result.Invalid(new ValidationError("Reservation already applied"));

        var adjustment = _createAdjustment.CreateForOrderItemReservation(
            model.ToCreateForOrderItemReservationModel(reservation)
        );

        if (adjustment.IsInvalid())
            return Result.Invalid(adjustment.ValidationErrors);

        var adjustmentResult = inventory.PlaceAdjustment(adjustment);

        if (adjustmentResult.IsInvalid())
            return Result.Invalid(adjustmentResult.ValidationErrors);

        var inventoryAfterAdjustment = adjustmentResult.Value;

        return inventoryAfterAdjustment.AlterReservationStatus(reservation.Id, ReservationStatus.Applied);
    }
}
