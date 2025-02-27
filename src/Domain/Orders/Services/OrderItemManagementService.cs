using Domain.Orders.Aggregates;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Products.Entities;

namespace Domain.Orders.Services;

public sealed record CreateOrderItemModel(Order Order, Product Product, Quantity Quantity, Amount UnitPrice);

public sealed class OrderItemManagementService
{
    public Result<OrderItem> CreateItem(CreateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var (order, product, quantity, unitPrice) = model;

        if (order.OrderItems.Any(item => item.Product.Id == product.Id))
            return Result.Invalid(new ValidationError($"There's already an item with product {product.Id}"));

        if (quantity.Value <= 0)
            return Result.Invalid(new ValidationError("Item quantity must be higher than 0"));

        if (unitPrice.Value <= 0)
            return Result.Invalid(new ValidationError("Item unit price must be higher than 0"));

        if (product.HasInventory && !product.Inventory!.HasEnoughStockToDecrease(quantity))
        {
            return Result.Invalid(
                new ValidationError(
                    $"Product '{product.Id}' has not enough stock for placing the order "
                        + $"({product.Inventory.GetAvailableStock().Value})"
                )
            );
        }

        return new OrderItem(
            new OrderItemId(Guid.NewGuid()),
            order.Id,
            product,
            null,
            new OrderItemPrice(quantity.Value, unitPrice.Value)
        );
    }
}
