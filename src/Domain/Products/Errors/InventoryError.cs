using Domain.Products.Aggregate;

namespace Domain.Products.Errors;

internal static class InventoryError
{
    internal static Error Inv001ProductAlreadyHasAnInventory(InventoryId inventoryId)
    {
        return new Error("INV001", $"Product already has an inventory ({inventoryId})");
    }

    internal static Error Inv002ProductIdMustBeInformed => new("INV002", $"Product ID must be informed");

    internal static Error Inv003QuantityToDecreaseIsGreaterThanAvailableStock(Product product)
    {
        return new Error(
            "INV003",
            $"Quantity to decrease is greater than available stock ({product.GetAvailableStock().Object})"
        );
    }

    internal static Error Inv004ReservationWithIdNotFound(ReservationId reservationId)
    {
        return new Error("INV004", $"Reservation with ID '{reservationId}' not found");
    }

    internal static Error Inv005ReservationWithOrderItemIdNotFound(OrderItemId orderItemId)
    {
        return new Error("INV005", $"Reservation with ID '{orderItemId}' not found");
    }

    internal static Error Inv006CanNotApplyIfReservationIsCancelled =>
        new("INV006", "Can not apply if reservation is cancelled");

    internal static Error Inv007ReservationAlreadyApplied => new("INV007", "Reservation already applied");
}
