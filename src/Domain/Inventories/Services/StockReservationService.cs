using Domain.Inventories.Aggregate;
using Domain.Inventories.Entities;
using Domain.Inventories.Enums;

namespace Domain.Inventories.Services;

public sealed record ReserveStockModel(Inventory Inventory, OrderItemId OrderItemId, Quantity Quantity);

public sealed partial class StockReservationService
{
    public Result<Inventory> ReserveStock(ReserveStockModel model)
    {
        var (inventory, orderItemId, quantity) = model;

        if (quantity.Value <= 0)
            return Result.Invalid(new ValidationError("Quantity must be greater than 0"));

        if (!inventory.HasEnoughStockToDecrease(quantity))
        {
            return Result.Invalid(
                new ValidationError(
                    $"Reservation quantity ({quantity.Value}) is greater than the available stock "
                        + $"({inventory.GetAvailableStock().Value})"
                )
            );
        }

        var reservation = Reservation.Create(
            new ReservationId(Guid.NewGuid()),
            inventory.Id,
            orderItemId,
            quantity,
            ReservationStatus.Pending
        );

        if (reservation.IsInvalid())
            return Result.Invalid(reservation.ValidationErrors!);

        return inventory.PlaceReservation(reservation);
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
            return Result.Invalid(new ValidationError($"Reservation for order item '{orderItemId}' not found"));

        if (reservation.Status is not ReservationStatus.Pending)
            return Result.Invalid(new ValidationError("Reservation status must be pending"));

        return inventory.AlterReservationStatus(reservation.Id, ReservationStatus.Cancelled);
    }
}
