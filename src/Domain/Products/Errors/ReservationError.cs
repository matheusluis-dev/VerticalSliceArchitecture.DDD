using Domain.Products.Aggregate;

namespace Domain.Products.Errors;

internal static class ReservationError
{
    internal static Error Res001InventoryIdMustBeInformed => new("RES001", "Inventory ID must be informed");
    internal static Error Res002OrderItemIdMustBeInformed => new("RES002", "Order Item ID must be informed");

    internal static Error Res003ReservationQuantityIsGreaterThanTheAvailableStock(Product product, Quantity quantity)
    {
        return new Error(
            "RES003",
            $"Reservation quantity ({quantity.Value}) is greater than the available stock "
                + $"({product.GetAvailableStock().Object})"
        );
    }

    internal static Error Res004OrderItemWithIdNotFound(OrderItemId orderItemId)
    {
        return new Error("RES004", $"Reservation for Order Item with ID '{orderItemId}' not found");
    }

    internal static Error Res005ReservationStatusMustBePending => new("RES005", "Reservation status must be pending");
}
