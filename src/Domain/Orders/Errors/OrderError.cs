using Domain.Products.Entities;

namespace Domain.Orders.Errors;

internal static class OrderError
{
    internal static Error Ord001CreatedDateMustBeInformed => new("ORD001", "Created date must be informed");
    internal static Error Ord002CanNotPayCancelledOrder => new("ORD002", "Can not pay cancelled order");
    internal static Error Ord003OrderAlreadyPaid => new("ORD003", "Order already paid");
    internal static Error Ord004OrderMustBePending => new("ORD004", "Order must pending");
    internal static Error Ord005OrderItemsMustBeProvided => new("ORD005", "Order Items must be provided");

    internal static Error Ord006TheresAlreadyAnItemWithProduct(ProductId productId)
    {
        return new Error("ORD006", $"There's already an item with product {productId}");
    }

    internal static Error Ord007ProductHasNotEnoughStockForPlacingTheOrder(Product product)
    {
        return new Error(
            "ORD007",
            $"Product '{product.Id}' has not enough stock for placing the order "
                + $"({product.Inventory!.GetAvailableStock().Value})"
        );
    }
}
