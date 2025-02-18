namespace Domain.Orders.Services;

using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Domain.Common.ValueObjects;
using Domain.Orders.Aggregates;
using Domain.Orders.Enums;
using Domain.Orders.Events;
using Domain.Orders.ValueObjects;
using Domain.Products.Entities;

public sealed record OrderPlacementModel(
    IEnumerable<OrderItemPlacementModel> Items,
    Email CustomerEmail,
    DateTime Now
);

public sealed record OrderItemPlacementModel(Product Product, Quantity Quantity, Amount UnitPrice)
{
    internal CreateOrderItemModel ToCreateOrderItemModel(Order order)
    {
        return new CreateOrderItemModel(order, Product, Quantity, UnitPrice);
    }
}

public sealed class OrderPlacementService
{
    private readonly OrderItemManagementService _orderItemManagement;

    public OrderPlacementService(OrderItemManagementService orderItemManagement)
    {
        _orderItemManagement = orderItemManagement;
    }

    public Result<Order> Place(OrderPlacementModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var (items, customerEmail, now) = model;

        if (!items.Any())
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
            var addItem = order.AddItem(_orderItemManagement, item.ToCreateOrderItemModel(order));

            if (addItem.IsInvalid())
                errorsAddingItem.AddRange(addItem.ValidationErrors);
        }

        if (errorsAddingItem.Count > 0)
            return Result<Order>.Invalid(errorsAddingItem);

        order.RaiseDomainEvent(new OrderPlacedEvent(order));

        return order;
    }
}
