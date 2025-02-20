namespace Domain.Orders.Aggregates;

using Domain.Common.DomainEvents;
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
    public required IReadOnlyList<OrderItem> OrderItems { get; init; }
    public required OrderStatus Status { get; init; }
    public required Email CustomerEmail { get; init; }
    public required DateTime CreatedDate { get; init; }
    public required DateTime? PaidDate { get; init; }
    public required DateTime? CancelledDate { get; init; }

    private Order(IList<IDomainEvent>? domainEvents = null)
        : base(domainEvents ?? []) { }

    internal static Result<Order> Create(
        OrderId id,
        IEnumerable<OrderItem>? items,
        OrderStatus status,
        Email? customerEmail,
        DateTime? createdDate,
        DateTime? paidDate,
        DateTime? cancelledDate,
        IEnumerable<IDomainEvent>? domainEvents
    )
    {
        var errors = new List<ValidationError>();

        //if (items?.Any() != true)
        //    errors.Add(new ValidationError("Order items must be provided"));

        if (customerEmail is null)
            errors.Add(new ValidationError("Customer email must be informed"));

        if (createdDate is null)
            errors.Add(new ValidationError("Created date must be informed"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new Order(domainEvents?.ToList())
        {
            Id = id,
            OrderItems = (items?.ToList() ?? []).AsReadOnly(),
            Status = status,
            CustomerEmail = customerEmail!.Value,
            CreatedDate = createdDate!.Value,
            PaidDate = paidDate,
            CancelledDate = cancelledDate,
        };
    }

    public Result<Order> Pay(DateTime now)
    {
        if (Status is OrderStatus.Cancelled)
            return Result.Invalid(new ValidationError("Can not pay cancelled order"));

        if (Status is OrderStatus.Paid)
            return Result.Invalid(new ValidationError("Order already paid"));

        var order = new OrderBuilder().WithOrderToClone(this).WithStatus(OrderStatus.Paid).WithPaidDate(now).Build();

        if (order.IsInvalid())
            return Result.Invalid(order.ValidationErrors);

        RaiseDomainEvent(new OrderPaidEvent(order));

        return order;
    }

    public Result<Order> Cancel(DateTime now)
    {
        if (Status is not OrderStatus.Pending)
            return Result.Invalid(new ValidationError("Order must be pending"));

        var order = new OrderBuilder()
            .WithOrderToClone(this)
            .WithStatus(OrderStatus.Cancelled)
            .WithPaidDate(now)
            .Build();

        if (order.IsInvalid())
            return Result.Invalid(order.ValidationErrors);

        RaiseDomainEvent(new OrderCancelledEvent(order));

        return order;
    }

    public Amount GetTotalPrice()
    {
        return Amount.From(OrderItems.Sum(item => item.Quantity.Value * item.UnitPrice.Value));
    }

    internal Result<Order> AddItem(OrderItemManagementService orderItemManagement, CreateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(orderItemManagement);
        ArgumentNullException.ThrowIfNull(model);

        var createItem = orderItemManagement.CreateItem(model);

        if (createItem.IsInvalid())
            return Result.Invalid(createItem.ValidationErrors);

        var item = createItem.Value;

        var order = new OrderBuilder().WithOrderToClone(this).WithOrderItems(OrderItems).WithOrderItems(item).Build();

        if (order.IsInvalid())
            return Result.Invalid(order.ValidationErrors);

        return order;
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
