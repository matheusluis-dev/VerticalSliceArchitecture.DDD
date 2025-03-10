using Domain.Orders.Aggregates;
using Domain.Orders.Entities;
using Domain.Orders.Errors;
using Domain.Orders.ValueObjects;
using Domain.Products.Aggregate;

namespace Domain.Orders.Services;

public sealed record CreateOrderItemModel(Order Order, Product Product, Quantity Quantity, Amount UnitPrice);

public sealed class OrderItemManagementService
{
    public Result<OrderItem> CreateItem(CreateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var (order, product, quantity, unitPrice) = model;

        if (order.OrderItems.Any(item => item.ProductId == product.Id))
            return Result.Failure(OrderError.Ord006TheresAlreadyAnItemWithProduct(product.Id));

        if (product.HasInventory && !product.HasEnoughStockToDecrease(quantity))
            return Result.Failure(OrderError.Ord007ProductHasNotEnoughStockForPlacingTheOrder(product));

        return new OrderItem(
            new OrderItemId(Guid.NewGuid()),
            order.Id,
            product.Id,
            null,
            new Price(quantity.Value, unitPrice.Value)
        );
    }
}
