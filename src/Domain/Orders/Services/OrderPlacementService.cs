using Domain.Orders.Aggregates;
using Domain.Orders.Enums;
using Domain.Orders.Errors;
using Domain.Orders.Events;
using Domain.Products.Entities;

namespace Domain.Orders.Services;

public sealed record OrderPlacementModel(IEnumerable<OrderItemPlacementModel> Items, Email CustomerEmail);

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
    private readonly IDateTimeService _dateTimeService;

    public OrderPlacementService(OrderItemManagementService orderItemManagement, IDateTimeService dateTimeService)
    {
        _orderItemManagement = orderItemManagement;
        _dateTimeService = dateTimeService;
    }

    public Result<Order> Place(OrderPlacementModel model)
    {
        var (items, customerEmail) = model;

        var itemsList = items.ToList();
        if (itemsList.Count is 0)
            return Result.Failure(OrderError.Ord005OrderItemsMustBeProvided);

        var createOrder = OrderBuilder
            .Create()
            .WithNewId()
            .WithStatus(OrderStatus.PENDING)
            .WithCustomerEmail(customerEmail)
            .WithCreatedDate(_dateTimeService.UtcNow.DateTime)
            .Build();

        if (createOrder.Failed)
            return createOrder;

        var order = createOrder.Value!;

        var errorsAddingItem = new List<Error>();
        foreach (var item in itemsList)
        {
            var addItem = order.AddItem(_orderItemManagement, item.ToCreateOrderItemModel(order));

            if (addItem.Failed)
                errorsAddingItem.AddRange(addItem.Errors);
        }

        if (errorsAddingItem.Count > 0)
            return Result.Failure(errorsAddingItem);

        order.RaiseDomainEvent(new OrderPlacedEvent(order));

        return order;
    }
}
