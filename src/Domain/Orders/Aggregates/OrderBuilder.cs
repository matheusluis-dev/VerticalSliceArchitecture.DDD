using Domain.Orders.Entities;
using Domain.Orders.Enums;

namespace Domain.Orders.Aggregates;

public sealed class OrderBuilder
{
    private Order? _orderToClone;

    private OrderId? _id;
    private readonly List<OrderItem> _orderItems = [];
    private OrderStatus? _status;
    private Email? _customerEmail;
    private DateTime? _createdDate;
    private DateTime? _paidDate;
    private DateTime? _cancelledDate;

    public Result<Order> Build()
    {
        var id = _orderToClone?.Id ?? _id ?? new OrderId(Guid.NewGuid());

        var orderItems = _orderItems.Count > 0 ? _orderItems : _orderToClone?.OrderItems;

        var status = _status ?? _orderToClone?.Status ?? OrderStatus.PENDING;
        var email = _customerEmail ?? _orderToClone?.CustomerEmail;
        var createdDate = _orderToClone?.CreatedDate ?? _createdDate;
        var paidDate = _paidDate ?? _orderToClone?.PaidDate;
        var cancelledDate = _cancelledDate ?? _orderToClone?.CancelledDate;
        var domainEvents = _orderToClone?.GetDomainEvents() ?? [];

        return Order.Create(id, orderItems, status, email, createdDate, paidDate, cancelledDate, domainEvents);
    }

    public OrderBuilder WithOrderToClone(Order order)
    {
        _orderToClone = order;
        return this;
    }

    public OrderBuilder WithId(OrderId id)
    {
        _id = id;
        return this;
    }

    public OrderBuilder WithOrderItems(params IEnumerable<OrderItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _orderItems.AddRange(items);

        return this;
    }

    public OrderBuilder WithStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    public OrderBuilder WithCustomerEmail(Email customerEmail)
    {
        _customerEmail = customerEmail;
        return this;
    }

    public OrderBuilder WithCreatedDate(DateTime createdDate)
    {
        _createdDate = createdDate;
        return this;
    }

    public OrderBuilder WithPaidDate(DateTime? paidDate)
    {
        _paidDate = paidDate;
        return this;
    }

    public OrderBuilder WithCanceledDate(DateTime? canceledDate)
    {
        _cancelledDate = canceledDate;
        return this;
    }
}
