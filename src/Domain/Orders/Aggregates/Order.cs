using System.Collections.Immutable;
using Domain.Orders.Entities;
using Domain.Orders.Enums;
using Domain.Orders.Errors;
using Domain.Orders.Events;
using Domain.Orders.Services;

namespace Domain.Orders.Aggregates;

public sealed class Order : AggregateBase
{
    public required OrderId Id { get; init; }
    public required IImmutableList<OrderItem> OrderItems { get; init; }
    public required OrderStatus Status { get; init; }
    public required Email CustomerEmail { get; init; }
    public required DateTime CreatedDate { get; init; }
    public required DateTime? PaidDate { get; init; }
    public required DateTime? CancelledDate { get; init; }

    private Order(IImmutableList<IDomainEvent>? domainEvents = null)
        : base(domainEvents ?? []) { }

    internal static Result<Order> Create(
        OrderId id,
        IImmutableList<OrderItem> items,
        OrderStatus status,
        Email customerEmail,
        DateTime? createdDate,
        DateTime? paidDate,
        DateTime? cancelledDate,
        IImmutableList<IDomainEvent>? domainEvents
    )
    {
        if (createdDate is null)
            return Result.Failure(OrderError.Ord001CreatedDateMustBeInformed);

        return new Order(domainEvents)
        {
            Id = id,
            OrderItems = items,
            Status = status,
            CustomerEmail = customerEmail,
            CreatedDate = createdDate.Value,
            PaidDate = paidDate,
            CancelledDate = cancelledDate,
        };
    }

    public Result<Order> Pay(IDateTimeService dateTime)
    {
        ArgumentNullException.ThrowIfNull(dateTime);

        if (Status is OrderStatus.CANCELLED)
            return Result.Failure(OrderError.Ord002CanNotPayCancelledOrder);

        if (Status is OrderStatus.PAID)
            return Result.Failure(OrderError.Ord003OrderAlreadyPaid);

        var order = OrderBuilder
            .Create()
            .WithOrderToClone(this)
            .WithStatus(OrderStatus.PAID)
            .WithPaidDate(dateTime.UtcNow.DateTime)
            .Build();

        if (order.Failed)
            return Result.Failure(order.Errors);

        RaiseDomainEvent(new OrderPaidEvent(order.Object!));

        return order;
    }

    public Result<Order> Cancel(IDateTimeService dateTime)
    {
        ArgumentNullException.ThrowIfNull(dateTime);

        if (Status is not OrderStatus.PENDING)
            return Result.Failure(OrderError.Ord004OrderMustBePending);

        var order = OrderBuilder
            .Create()
            .WithOrderToClone(this)
            .WithStatus(OrderStatus.CANCELLED)
            .WithPaidDate(dateTime.UtcNow.DateTime)
            .Build();

        if (order.Failed)
            return order;

        order.Object!.RaiseDomainEvent(new OrderCancelledEvent(order.Object!));

        return order;
    }

    internal Result<Order> AddItem(OrderItemManagementService orderItemManagement, CreateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(orderItemManagement);
        ArgumentNullException.ThrowIfNull(model);

        var createItem = orderItemManagement.CreateItem(model);

        if (createItem.Failed)
            return Result.Failure(createItem.Errors);

        var item = createItem.Object!;

        return OrderBuilder.Create().WithOrderToClone(this).WithOrderItem(item).Build();
    }
}
