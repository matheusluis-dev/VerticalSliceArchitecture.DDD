using Domain.Inventories.Aggregate;
using Domain.Inventories.Entities;
using Domain.Inventories.Enums;
using Domain.Inventories.Errors;

namespace Domain.Inventories.Services;

public sealed record ReleaseStockReservationModel(Inventory Inventory, OrderItemId OrderItemId)
{
    public CreateForOrderItemReservationModel ToCreateForOrderItemReservationModel(Reservation reservation)
    {
        return new CreateForOrderItemReservationModel(OrderItemId, reservation);
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
            return Result.Failure(InventoryError.Inv005ReservationWithOrderItemIdNotFound(orderItemId));

        if (reservation.Status is ReservationStatus.CANCELLED)
            return Result.Failure(InventoryError.Inv006CanNotApplyIfReservationIsCancelled);

        if (reservation.Status is ReservationStatus.APPLIED)
            return Result.Failure(InventoryError.Inv007ReservationAlreadyApplied);

        var adjustment = _createAdjustment.CreateForOrderItemReservation(
            model.ToCreateForOrderItemReservationModel(reservation)
        );

        if (adjustment.Failed)
            return Result.Failure(adjustment.Errors);

        var adjustmentResult = inventory.PlaceAdjustment(adjustment.Value!);

        if (adjustmentResult.Failed)
            return Result.Failure(adjustmentResult.Errors);

        var inventoryAfterAdjustment = adjustmentResult.Value!;

        return inventoryAfterAdjustment.AlterReservationStatus(reservation.Id, ReservationStatus.APPLIED);
    }
}
