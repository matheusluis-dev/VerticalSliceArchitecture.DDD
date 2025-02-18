namespace Domain.Inventories.Services;

using Domain.Common.ValueObjects;
using Domain.Inventories.Aggregate;
using Domain.Inventories.Entities;
using Domain.Inventories.Enums;
using Domain.Inventories.Specifications;
using Domain.Inventories.ValueObjects;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;

public sealed record ReserveStockModel(
    Inventory Inventory,
    OrderItemId OrderItemId,
    Quantity Quantity
);

public sealed partial class StockReservationService
{
    public Result<Reservation> ReserveStock(ReserveStockModel model)
    {
        var (inventory, orderItemId, quantity) = model;

        if (quantity.Value <= 0)
        {
            return Result<Reservation>.Invalid(
                new ValidationError("Quantity must be greater than 0")
            );
        }

        if (!new HasEnoughStockToDecreaseSpecification(quantity).IsSatisfiedBy(inventory))
        {
            return Result<Reservation>.Invalid(
                new ValidationError(
                    $"Reservation quantity ({quantity}) is greater than the available stock ({inventory.GetAvailableStock()})"
                )
            );
        }

        var reservation = new Reservation
        {
            Id = ReservationId.Create(),
            InventoryId = inventory.Id,
            OrderItemId = orderItemId,
            Quantity = quantity,
            Status = ReservationStatus.Pending,
        };

        inventory.PlaceReservation(reservation);

        return reservation;
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
        {
            return Result<Inventory>.Invalid(
                new ValidationError($"Reservation for order item '{orderItemId}' not found")
            );
        }

        if (reservation.Status is not ReservationStatus.Pending)
        {
            return Result<Inventory>.Invalid(
                new ValidationError("Reservation status must be pending")
            );
        }

        reservation.Status = ReservationStatus.Cancelled;

        return inventory;
    }
}





