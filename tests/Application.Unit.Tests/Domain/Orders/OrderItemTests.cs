namespace Application.Unit.Tests.Domain.Orders;

using Application.Common.Tests.Domain.Orders;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Services.UpdateOrder.Models;
using Application.Domain.Orders.ValueObjects;
using Ardalis.Result;

public sealed class OrderItemTests
{
    [Fact]
    public void Add_order_item()
    {
        // Arrange
        var order = OrderFaker.Generate(OrderFakerConfiguration.Create().SetOrderCount(1)).First();

        // Act
        var result = order.AddOrderItem(
            new OrderItem
            {
                Id = OrderItemId.From(Guid.NewGuid()),
                OrderId = order.Id,
                Quantity = Quantity.From(1),
                UnitPrice = Amount.From(1),
            }
        );

        // Assert
        Check.That(order.OrderItems).CountIs(1);
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.IsCreated()).IsTrue();
    }

    [Fact]
    public void Add_order_item_with_duplicated_ID_must_not_be_allowed()
    {
        // Arrange
        var order = OrderFaker
            .Generate(OrderFakerConfiguration.Create().SetOrderCount(1).SetOrderItemCount(1))
            .First();
        var existentItemId = order.OrderItems[0].Id;

        // Act
        var result = order.AddOrderItem(
            new OrderItem
            {
                Id = existentItemId,
                OrderId = order.Id,
                Quantity = Quantity.From(1),
                UnitPrice = Amount.From(1),
            }
        );

        // Assert
        Check.That(result.IsSuccess).IsFalse();
    }

    [Fact]
    public void Update_order_item()
    {
        // Arrange
        var oldPrice = Amount.From(50);
        var newPrice = Amount.From(100);

        var order = OrderFaker
            .Generate(
                OrderFakerConfiguration
                    .Create()
                    .SetOrderCount(1)
                    .SetOrderItemCount(1)
                    .SetOrderItemPrice(oldPrice.Value, [(0, 0)])
            )
            .First();
        var item = order.OrderItems[0];

        var oldItemQuantity = item.Quantity;
        var oldItemPrice = item.UnitPrice;

        // Act
        order.UpdateOrderItem(
            new UpdateOrderItemModel(item.Id, Quantity: null, UnitPrice: newPrice)
        );

        // Assert
        Check.That(order.OrderItems[0].UnitPrice).HasDifferentValueThan(oldItemPrice);
        Check.That(order.OrderItems[0].Quantity).IsEqualTo(oldItemQuantity);
    }

    [Fact]
    public void Update_inexistent_order_must_result_false()
    {
        // Arrange
        var oldPrice = Amount.From(50);
        var newPrice = Amount.From(100);

        var order = OrderFaker
            .Generate(
                OrderFakerConfiguration
                    .Create()
                    .SetOrderCount(1)
                    .SetOrderItemPrice(oldPrice.Value, [(0, 0)])
            )
            .First();
        var nonExistentId = OrderItemId.From(Guid.NewGuid());

        // Act
        var result = order.UpdateOrderItem(
            new UpdateOrderItemModel(nonExistentId, Quantity: null, UnitPrice: newPrice)
        );

        // Assert
        Check.That(result.IsSuccess).IsFalse();
    }
}
