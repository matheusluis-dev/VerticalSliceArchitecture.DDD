using Domain.Products.Aggregate;
using Domain.Products.Entities;
using Domain.Products.Enums;
using Domain.Products.Errors;

namespace Domain.Products.Services;

public sealed record ReleaseStockReservationModel(Product Product, OrderItemId OrderItemId)
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

    public Result<Product> ReleaseStockReservation(ReleaseStockReservationModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var (product, orderItemId) = model;

        var reservation = product.GetReservations().FirstOrDefault(r => r.OrderItemId == orderItemId);

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

        var adjustmentResult = product.PlaceAdjustment(adjustment.Object!);

        if (adjustmentResult.Failed)
            return Result.Failure(adjustmentResult.Errors);

        var inventoryAfterAdjustment = adjustmentResult.Object!;
        return inventoryAfterAdjustment.AlterReservationStatus(reservation.Id, ReservationStatus.APPLIED);
    }
}
