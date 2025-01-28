namespace Application.Common.Tests.Domain.Orders;

using System.Diagnostics.CodeAnalysis;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.ValueObjects;
using Ardalis.GuardClauses;
using LinqKit;

public static class OrderFaker
{
    public static IEnumerable<Order> Generate(OrderFakerConfiguration configuration)
    {
        Guard.Against.Null(configuration);

        var orderQuantity = configuration.OrderQuantity ?? 1;

        for (var orderIndex = 0; orderIndex < orderQuantity; orderIndex++)
        {
            var orderId = OrderId.From(Guid.NewGuid());
            var status = GetOrderStatusForCurrent(configuration, orderIndex);
            var items = GenerateItems(configuration, orderIndex, orderId);

            var orderFaker = new Faker<Order>("en_US")
                .RuleFor(order => order.Id, _ => orderId)
                .RuleFor(order => order.Status, _ => status)
                .RuleFor(order => order.Created, faker => faker.Date.Past())
                .RuleFor(order => order.CreatedBy, faker => UserName.From(faker.Name.FullName()))
                .RuleFor(order => order.LastModified, faker => faker.Date.Future())
                .RuleFor(
                    order => order.LastModifiedBy,
                    faker => UserName.From(faker.Name.FullName())
                );

            var order = orderFaker.Generate(1)[0];
            items.ForEach(item => order.AddOrderItem(item));

            yield return order;
        }
    }

    private static OrderStatus GetOrderStatusForCurrent(
        [NotNull] OrderFakerConfiguration configuration,
        int orderIndex
    )
    {
        if (!configuration.Statuses.TryGetValue(orderIndex, out var status))
            return OrderStatus.Pending;

        return status;
    }

    private static IEnumerable<OrderItem> GenerateItems(
        [NotNull] OrderFakerConfiguration configuration,
        int orderIndex,
        OrderId orderId
    )
    {
        var itemQuantity = GetCurrentOrderItemQuantity(configuration, orderIndex);

        for (var orderItemIndex = 0; orderItemIndex < itemQuantity; orderItemIndex++)
        {
            var price = GetCurrentItemPrice(configuration, orderIndex, orderItemIndex);

            var orderItemFaker = new Faker<OrderItem>()
                .RuleFor(item => item.Id, _ => OrderItemId.From(Guid.NewGuid()))
                .RuleFor(item => item.OrderId, _ => orderId)
                .RuleFor(item => item.Quantity, _ => Quantity.From(1))
                .RuleFor(item => item.UnitPrice, _ => Amount.From(price));

            yield return orderItemFaker.Generate(1)[0];
        }
    }

    private static int GetCurrentOrderItemQuantity(
        [NotNull] OrderFakerConfiguration configuration,
        int orderIndex
    )
    {
        if (!configuration.ItemsQuantities.TryGetValue(orderIndex, out var quantity))
            return 0;

        return quantity;
    }

    private static decimal GetCurrentItemPrice(
        [NotNull] OrderFakerConfiguration configuration,
        int orderIndex,
        int orderItemIndex
    )
    {
        if (!configuration.ItemsPrices.TryGetValue((orderIndex, orderItemIndex), out var price))
            return 0;

        return price;
    }
}
