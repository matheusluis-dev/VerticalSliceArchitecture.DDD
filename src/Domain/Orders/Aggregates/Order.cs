namespace Domain.Orders.Aggregates;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Orders.Entities;
using Domain.Orders.Enums;
using Domain.Orders.Events;
using Domain.Orders.Services;
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
    public required OrderStatus Status { get; set; }
    public required Email CustomerEmail { get; init; }
    public required DateTime CreatedDate { get; init; }
    public DateTime? PaidDate { get; set; }
    public DateTime? CanceledDate { get; set; }

    public Result<Order> Pay(DateTime now)
    {
        if (Status is OrderStatus.Cancelled)
            return Result.Invalid(new ValidationError("Can not pay cancelled order"));

        if (Status is OrderStatus.Paid)
            return Result.Invalid(new ValidationError("Order already paid"));

        Status = OrderStatus.Paid;
        PaidDate = now;

        RaiseDomainEvent(new OrderPaidEvent(this));

        return this;
    }

    public Result<Order> Cancel(DateTime now)
    {
        if (Status is not OrderStatus.Pending)
            return Result.Invalid(new ValidationError("Order must be pending"));

        Status = OrderStatus.Cancelled;
        CanceledDate = now;

        RaiseDomainEvent(new OrderCancelledEvent(this));

        return this;
    }

    public Amount GetTotalPrice()
    {
        return Amount.From(OrderItems.Sum(item => item.Quantity.Value * item.UnitPrice.Value));
    }

    internal Result<OrderItem> AddItem(
        OrderItemManagementService orderItemManagement,
        CreateOrderItemModel model
    )
    {
        ArgumentNullException.ThrowIfNull(orderItemManagement);
        ArgumentNullException.ThrowIfNull(model);

        var result = orderItemManagement.CreateItem(model);

        if (!result.IsSuccess)
            return result;

        var item = result.Value;
        _orderItems.Add(item);

        return Result.Created(item);
    }

    //public Result<OrderItem> UpdateItem(UpdateOrderItemModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    var id = model.Id;
    //    var item = OrderItems.FirstOrDefault(item => item.Id == id);

    //    if (item is null)
    //        return Result.NotFound($"Item with ID {model.Id} not found");

    //    var itemIndex = _orderItems.IndexOf(item);

    //    _orderItems[itemIndex] = new OrderItem
    //    {
    //        OrderId = item.OrderId,
    //        Id = model.Id,
    //        Quantity = model.Quantity ?? item.Quantity,
    //        UnitPrice = model.UnitPrice ?? item.UnitPrice,
    //        Product = item.Product,
    //        ReservationId = item.ReservationId,
    //    };

    //    return Result.Success(item);
    //}

    //public Result DeleteItem(OrderItemId itemId)
    //{
    //    var item = _orderItems.FirstOrDefault(item => item.Id == itemId);

    //    if (item is null)
    //        return Result.NotFound($"Item with ID {itemId} not found");

    //    _orderItems.Remove(item);

    //    return Result.Success();
    //}
}
