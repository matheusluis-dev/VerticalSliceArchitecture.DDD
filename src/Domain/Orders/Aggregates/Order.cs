namespace Domain.Orders.Aggregates;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.Specifications;
using Domain.Orders.Entities;
using Domain.Orders.Enums;
using Domain.Orders.Events;
using Domain.Orders.Models;
using Domain.Orders.Services.UpdateOrder.Models;
using Domain.Orders.ValueObjects;

public sealed class Order : EntityBase
{
    public required OrderId Id { get; init; }

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems
    {
        get => _orderItems.AsReadOnly();
        init => _orderItems = [.. value];
    }
    public required OrderStatus Status { get; init; }
    public required Email CustomerEmail { get; init; }
    public required DateTime CreatedDate { get; init; }
    public DateTime? PaidDate { get; init; }
    public DateTime? CanceledDate { get; init; }

    public static Result<Order> Place(
        IList<AddOrderItemModel> items,
        Email customerEmail,
        DateTime now
    )
    {
        ArgumentNullException.ThrowIfNull(items);

        if (items.Count == 0)
            return Result<Order>.Invalid(new ValidationError("Items must be provided"));

        var order = new Order
        {
            Id = OrderId.Create(),
            Status = OrderStatus.Pending,
            CustomerEmail = customerEmail,
            CreatedDate = now,
            CanceledDate = null,
            PaidDate = null,
        };

        var errorsAddingItem = new List<ValidationError>();
        foreach (var item in items)
        {
            var addItem = order.AddItem(item);

            if (addItem.IsInvalid())
                errorsAddingItem.AddRange(addItem.ValidationErrors);
        }

        if (errorsAddingItem.Count > 0)
            return Result<Order>.Invalid(errorsAddingItem);

        order.RaiseDomainEvent(new OrderPlacedEvent(order));

        return order;
    }

    public Amount GetTotalPrice()
    {
        return Amount.From(OrderItems.Sum(item => item.Quantity.Value * item.UnitPrice.Value));
    }

    public Result<OrderItem> AddItem(AddOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (OrderItems.Any(item => item.Product.Id == model.Product.Id))
        {
            return Result<OrderItem>.Invalid(
                new ValidationError($"There's already an item with product {model.Product.Id}")
            );
        }

        if (model.Quantity.Value <= 0)
        {
            return Result<OrderItem>.Invalid(
                new ValidationError("Item quantity must be higher than 0")
            );
        }

        if (model.UnitPrice.Value <= 0)
        {
            return Result<OrderItem>.Invalid(
                new ValidationError("Item unit price must be higher than 0")
            );
        }

        if (
            model.Product.Inventory is not null
            && !new HasEnoughStockToDecreaseSpecification(model.Quantity).IsSatisfiedBy(
                model.Product.Inventory
            )
        )
        {
            return Result<OrderItem>.Invalid(
                new ValidationError(
                    $"Product '{model.Product.Id}' has not enough stock for placing the order "
                        + $"({model.Product.Inventory.GetAvailableStock()})"
                )
            );
        }

        var item = new OrderItem
        {
            Id = OrderItemId.Create(),
            OrderId = Id,
            Product = model.Product,
            Quantity = model.Quantity,
            UnitPrice = model.UnitPrice,
        };

        _orderItems.Add(item);

        return Result.Created(item);
    }

    public Result<OrderItem> UpdateItem(UpdateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var id = model.Id;
        var item = OrderItems.FirstOrDefault(item => item.Id == id);

        if (item is null)
            return Result.NotFound($"Item with ID {model.Id} not found");

        var itemIndex = _orderItems.IndexOf(item);

        _orderItems[itemIndex] = new OrderItem
        {
            OrderId = item.OrderId,
            Id = model.Id,
            Quantity = model.Quantity ?? item.Quantity,
            UnitPrice = model.UnitPrice ?? item.UnitPrice,
            Product = item.Product,
            ReservationId = item.ReservationId,
        };

        return Result.Success(item);
    }

    public Result DeleteItem(OrderItemId itemId)
    {
        var item = _orderItems.FirstOrDefault(item => item.Id == itemId);

        if (item is null)
            return Result.NotFound($"Item with ID {itemId} not found");

        _orderItems.Remove(item);

        return Result.Success();
    }
}
