namespace Application.Unit.Tests.Domain.Orders;

using System.Linq.Expressions;
using Application.Common.Tests.Domain.Orders;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.Specifications.Builder;

public sealed class OrderSpecificationTests
{
    private IOrderSpecificationBuilderCriteria OrderSpecificationBuilderFactory(
        IQueryable<Order> queryable
    )
    {
        IQueryable<Order> Callback(Expression<Func<Order, bool>> predicate)
        {
            return queryable.Where(predicate);
        }

        return new OrderSpecificationBuilder().SetQueryableCallback(Callback);
    }

    [Fact]
    public void Filter_only_paid_orders()
    {
        // Arrange
        var orders = OrderFaker
            .Generate(
                OrderFakerConfiguration
                    .Create()
                    .SetOrderCount(5)
                    .SetOrderStatus(OrderStatus.Pending, orderIndexes: [1, 3])
                    .SetOrderStatus(OrderStatus.Paid, orderIndexes: [0, 2, 4])
                    .SetOrderItemCount(1)
            )
            .AsQueryable();

        var builder = OrderSpecificationBuilderFactory(orders);

        // Act
        var filtered = builder.ArePaid().GetQueryable();

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
                    .SetOrderCount(5)
                    .SetOrderItemCount(2)
                    .SetOrderItemPrice(700, indexes: [(0, 0), (0, 1)])
                    .SetOrderItemPrice(2000, indexes: [(1, 0)])
                    .SetOrderItemPrice(0, indexes: [(1, 1)])
                    .SetOrderItemPrice(1000, indexes: [(2, 0)])
                    .SetOrderItemPrice(0, indexes: [(2, 1)])
            )
            .AsQueryable();

        var priceFilter = Amount.From(1000);
        var builder = OrderSpecificationBuilderFactory(orders);

        // Act
        var filtered = builder.TotalPriceHigherThan(priceFilter).GetQueryable();

        // Assert
        Check.That(filtered).CountIs(2);
        Check
            .That(filtered)
            .ContainsOnlyElementsThatMatch(order => order.TotalPrice.Value > priceFilter.Value);
    }

    [Fact]
    public void Filter_orders_paid_or_price_higher_than_50()
    {
        // Arrange
        var orders = OrderFaker
            .Generate(
                OrderFakerConfiguration
                    .Create()
                    .SetOrderCount(2)
                    .SetOrderItemCount(1)
                    .SetOrderStatus(OrderStatus.Pending, orderIndexes: [0])
                    .SetOrderItemPrice(51, [(0, 0)])
                    .SetOrderStatus(OrderStatus.Paid, orderIndexes: [1])
                    .SetOrderItemPrice(1, [(1, 0)])
            )
            .AsQueryable();

        var builder = OrderSpecificationBuilderFactory(orders);
        var priceFilter = Amount.From(50);

        // Act
        var filtered = builder.ArePaid().Or().TotalPriceHigherThan(priceFilter).GetQueryable();

        // Assert
        Check.That(filtered).CountIs(2);
    }
}
