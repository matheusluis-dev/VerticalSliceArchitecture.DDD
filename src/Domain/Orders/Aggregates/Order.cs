using Domain.Orders.Entities;
using Domain.Orders.Enums;
using Domain.Orders.Events;
using Domain.Orders.Services;

namespace Domain.Orders.Aggregates;

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
            CustomerEmail = customerEmail!,
            CreatedDate = createdDate!.Value,
            PaidDate = paidDate,
            CancelledDate = cancelledDate,
        };
    }

    public Result<Order> Pay(DateTime now)
    {
        if (Status == OrderStatus.CANCELLED)
            return Result.Invalid(new ValidationError("Can not pay cancelled order"));

        if (Status == OrderStatus.PAID)
            return Result.Invalid(new ValidationError("Order already paid"));

        var order = new OrderBuilder().WithOrderToClone(this).WithStatus(OrderStatus.PAID).WithPaidDate(now).Build();

        if (order.IsInvalid())
            return Result.Invalid(order.ValidationErrors!);

        RaiseDomainEvent(new OrderPaidEvent(order));

        return order;
    }

    public Result<Order> Cancel(DateTime now)
    {
        if (Status is not OrderStatus.PENDING)
            return Result.Invalid(new ValidationError("Order must be pending"));

        var order = new OrderBuilder()
            .WithOrderToClone(this)
            .WithStatus(OrderStatus.CANCELLED)
            .WithPaidDate(now)
            .Build();

        if (order.IsInvalid())
            return Result.Invalid(order.ValidationErrors!);

        RaiseDomainEvent(new OrderCancelledEvent(order));

        return order;
    }

    internal Result<Order> AddItem(OrderItemManagementService orderItemManagement, CreateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(orderItemManagement);
        ArgumentNullException.ThrowIfNull(model);

        var createItem = orderItemManagement.CreateItem(model);

        if (createItem.IsInvalid())
            return Result.Invalid(createItem.ValidationErrors!);

        var item = createItem.Value!;

        var order = new OrderBuilder().WithOrderToClone(this).WithOrderItems(OrderItems).WithOrderItems(item).Build();

        return order.IsInvalid() ? Result.Invalid(order.ValidationErrors!) : order;
    }
}
