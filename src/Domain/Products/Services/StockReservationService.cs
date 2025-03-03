using Domain.Products.Entities;
using Domain.Products.Enums;
using Domain.Products.Errors;

namespace Domain.Products.Services;

public sealed record ReserveStockModel(Inventory Inventory, OrderItemId OrderItemId, Quantity Quantity);

public sealed partial class StockReservationService
{
    public Result<Inventory> ReserveStock(ReserveStockModel model)
    {
        var (inventory, orderItemId, quantity) = model;

        if (!inventory.HasEnoughStockToDecrease(quantity))
        {
            return Result.Failure(
                ReservationError.Res003ReservationQuantityIsGreaterThanTheAvailableStock(inventory, quantity)
            );
        }

        var reservation = Reservation.Create(
            new ReservationId(GuidV7.NewGuid()),
            inventory.Id,
            orderItemId,
            quantity,
            ReservationStatus.PENDING
        );

        return reservation.Failed ? Result.Failure(reservation.Errors) : inventory.PlaceReservation(reservation.Value!);
    }
}

public sealed record CancelStockReservationModel(Inventory Inventory, OrderItemId OrderItemId);

public sealed partial class StockReservationService
{
    public Result<Inventory> CancelStockReservation(CancelStockReservationModel model)
    {
        var (inventory, orderItemId) = model;

        var reservation = inventory.Reservations.FirstOrDefault(r => r.OrderItemId == orderItemId);

        if (reservation is null)
            return Result.Failure(ReservationError.Res004OrderItemWithIdNotFound(orderItemId));

        if (reservation.Status is not ReservationStatus.PENDING)
            return Result.Failure(ReservationError.Res005ReservationStatusMustBePending);

        return inventory.AlterReservationStatus(reservation.Id, ReservationStatus.CANCELLED);
    }
}
