using System.Collections.Immutable;
using Domain.Orders.Entities;
using Domain.Orders.Enums;

namespace Domain.Orders.Aggregates;

public sealed class OrderBuilder : IOrderBuilder
{
    private Order? _orderToClone;

    private OrderId? _id;
    private IImmutableList<OrderItem> _orderItemsToAdd = [];
    private OrderStatus? _status;
    private Email? _customerEmail;
    private DateTime? _createdDate;
    private DateTime? _paidDate;
    private DateTime? _cancelledDate;

    private OrderBuilder() { }

    public static IOrderBuilderStart Create()
    {
        return new OrderBuilder();
    }

    public Result<Order> Build()
    {
        return _orderToClone is null ? New() : Clone();

        Result<Order> New()
        {
            if (_status is null)
                throw new ArgumentNullException(nameof(_status));

            ArgumentNullException.ThrowIfNull(_customerEmail);

            return Order.Create(
                _id ?? new OrderId(Guid.NewGuid()),
                _orderItemsToAdd,
                _status.Value,
                _customerEmail,
                _createdDate,
                _paidDate,
                _cancelledDate,
                null
            );
        }

        Result<Order> Clone()
        {
            return Order.Create(
                _orderToClone.Id,
                _orderToClone.OrderItems.AddRange(_orderItemsToAdd),
                _status ?? _orderToClone.Status,
                _customerEmail ?? _orderToClone.CustomerEmail,
                _orderToClone.CreatedDate,
                _orderToClone.PaidDate ?? _paidDate,
                _orderToClone.CancelledDate ?? _cancelledDate,
                _orderToClone.GetDomainEvents()
            );
        }
    }

    public IOrderBuilderWithPropertiesCloning WithOrderToClone(Order orderToClone)
    {
        ArgumentNullException.ThrowIfNull(orderToClone);

        _orderToClone = orderToClone;
        return this;
    }

    public IOrderBuilderWithSequence WithNewId()
    {
        _id = new OrderId(Guid.NewGuid());
        return this;
    }

    public IOrderBuilderWithSequence WithId(OrderId id)
    {
        _id = id;
        return this;
    }

    public IOrderBuilderWithSequence WithOrderItem(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        _orderItemsToAdd = _orderItemsToAdd.Add(orderItem);
        return this;
    }

    public IOrderBuilderWithSequence WithOrderItems(IEnumerable<OrderItem> orderItems)
    {
        ArgumentNullException.ThrowIfNull(orderItems);

        _orderItemsToAdd = _orderItemsToAdd.AddRange(orderItems);
        return this;
    }

    public IOrderBuilderWithSequence WithStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    public IOrderBuilderWithSequence WithCustomerEmail(Email customerEmail)
    {
        _customerEmail = customerEmail;
        return this;
    }

    public IOrderBuilderWithSequence WithCreatedDate(DateTime createdDate)
    {
        _createdDate = createdDate;
        return this;
    }

    public IOrderBuilderWithSequence WithPaidDate(DateTime? paidDate)
    {
        _paidDate = paidDate;
        return this;
    }

    public IOrderBuilderWithSequence WithCanceledDate(DateTime? canceledDate)
    {
        _cancelledDate = canceledDate;
        return this;
    }
}

public interface IOrderBuilder : IOrderBuilderStart, IOrderBuilderWithSequence, IOrderBuilderWithPropertiesCloning;

public interface IOrderBuilderStart : IOrderBuilderClone, IOrderBuilderWithId;

public interface IOrderBuilderClone
{
    IOrderBuilderWithPropertiesCloning WithOrderToClone(Order orderToClone);
}

public interface IOrderBuilderWithId
{
    IOrderBuilderWithSequence WithNewId();
    IOrderBuilderWithSequence WithId(OrderId id);
}

public interface IOrderBuilderWithProperties
{
    IOrderBuilderWithSequence WithOrderItem(OrderItem orderItem);
    IOrderBuilderWithSequence WithOrderItems(IEnumerable<OrderItem> orderItems);
    IOrderBuilderWithSequence WithStatus(OrderStatus status);
    IOrderBuilderWithSequence WithCustomerEmail(Email customerEmail);
    IOrderBuilderWithSequence WithCreatedDate(DateTime createdDate);
    IOrderBuilderWithSequence WithPaidDate(DateTime? paidDate);
    IOrderBuilderWithSequence WithCanceledDate(DateTime? canceledDate);
}

public interface IOrderBuilderWithPropertiesCloning : IOrderBuilderWithProperties { }

public interface IOrderBuilderWithSequence : IOrderBuilderWithProperties, IOrderBuilderBuild;

public interface IOrderBuilderBuild
{
    Result<Order> Build();
}
