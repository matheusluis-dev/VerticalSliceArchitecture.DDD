namespace Application.Unit.Tests.Domain.Orders;

using Application.Common.Tests.Domain.Orders;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.Specifications;

public sealed class OrderSpecificationTests
{
    [Fact]
    public void Filter_only_paid_orders()
    {
        // Arrange
        var orders = OrderFaker
            .Generate(
                OrderFakerConfiguration
                    .Create()
                    .SetOrderQuantity(5)
                    .SetOrderStatus(OrderStatus.Pending, orderIndexes: [1, 3])
                    .SetOrderStatus(OrderStatus.Paid, orderIndexes: [0, 2, 4])
                    .SetOrderItemQuantity(1)
            )
            .AsQueryable();

        var specification = new ArePaidSpecification();

        // Act
        var filtered = orders.Where(specification.Criteria);

        // Assert
        Check.That(filtered).CountIs(3);
        Check
            .That(filtered)
            .ContainsOnlyElementsThatMatch(order => order.Status is OrderStatus.Paid);
    }

    [Fact]
    public void Filter_orders_with_price_higher_than_1000()
    {
        // Arrange
        var orders = OrderFaker
            .Generate(
                OrderFakerConfiguration
                    .Create()
                    .SetOrderQuantity(5)
                    .SetOrderItemQuantity(2)
                    .SetOrderItemPrice(700, indexes: [(0, 0), (0, 1)])
                    .SetOrderItemPrice(2000, indexes: [(1, 0)])
                    .SetOrderItemPrice(0, indexes: [(1, 1)])
                    .SetOrderItemPrice(1000, indexes: [(2, 0)])
                    .SetOrderItemPrice(0, indexes: [(2, 1)])
            )
            .AsQueryable();

        var @value = 1000;
        var specification = new PriceHigherThanValueSpecification(@value);

        // Act
        var filtered = orders.Where(specification.Criteria);

        // Assert
        Check.That(filtered).CountIs(2);
        Check
            .That(filtered)
            .ContainsOnlyElementsThatMatch(order => order.TotalPrice.Value > @value);
    }
}
