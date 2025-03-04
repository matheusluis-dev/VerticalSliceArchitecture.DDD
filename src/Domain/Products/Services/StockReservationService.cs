using Domain.Products.Aggregate;
using Domain.Products.Entities;
using Domain.Products.Enums;
using Domain.Products.Errors;

namespace Domain.Products.Services;

public sealed record ReserveStockModel(Product Product, OrderItemId OrderItemId, Quantity Quantity);

public sealed partial class StockReservationService
{
    public Result<Product> ReserveStock(ReserveStockModel model)
    {
        var (product, orderItemId, quantity) = model;

        if (!product.HasEnoughStockToDecrease(quantity))
        {
            return Result.Failure(
                ReservationError.Res003ReservationQuantityIsGreaterThanTheAvailableStock(product, quantity)
            );
        }

        var reservation = Reservation.Create(
            new ReservationId(Guid.NewGuid()),
            product.InventoryId,
            orderItemId,
            quantity,
            ReservationStatus.PENDING
        );

        return reservation.Failed ? Result.Failure(reservation.Errors) : product.PlaceReservation(reservation.Object!);
    }
}

public sealed record CancelStockReservationModel(Product Inventory, OrderItemId OrderItemId);

public sealed partial class StockReservationService
{
    public Result<Product> CancelStockReservation(CancelStockReservationModel model)
    {
        var (product, orderItemId) = model;

        var reservation = product.GetReservations().FirstOrDefault(r => r.OrderItemId == orderItemId);

        if (reservation is null)
            return Result.Failure(ReservationError.Res004OrderItemWithIdNotFound(orderItemId));

        if (reservation.Status is not ReservationStatus.PENDING)
            return Result.Failure(ReservationError.Res005ReservationStatusMustBePending);

        return product.AlterReservationStatus(reservation.Id, ReservationStatus.CANCELLED);
    }
}
