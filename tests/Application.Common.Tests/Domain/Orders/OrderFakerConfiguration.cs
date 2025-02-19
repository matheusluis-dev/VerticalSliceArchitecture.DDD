namespace Application.Common.Tests.Domain.Orders;

using Ardalis.GuardClauses;
using global::Domain.Orders.Enums;

public sealed class OrderFakerConfiguration
{
    internal int? OrderQuantity { get; private set; }
    internal IDictionary<int, OrderStatus> Statuses { get; } = new Dictionary<int, OrderStatus>();
    internal IDictionary<int, int> ItemsQuantities { get; } = new Dictionary<int, int>();
    internal IDictionary<(int OrderIndex, int OrderItemIndex), decimal> ItemsPrices { get; } =
        new Dictionary<(int OrderIndex, int OrderItemIndex), decimal>();

    private OrderFakerConfiguration() { }

    public static OrderFakerConfiguration Create()
    {
        return new OrderFakerConfiguration();
    }

    public OrderFakerConfiguration SetOrderCount(int orderQuantity)
    {
        OrderQuantity = orderQuantity;
        return this;
    }

    public OrderFakerConfiguration SetOrderStatus(OrderStatus orderStatus, IEnumerable<int> orderIndexes)
    {
        Guard.Against.Null(orderIndexes);

        foreach (var index in orderIndexes)
        {
            if (Statuses.ContainsKey(index))
            {
                throw new InvalidOperationException(
                    $"{nameof(orderStatus)} for Order that has index {index} was already set"
                );
            }

            Statuses.Add(index, orderStatus);
        }

        return this;
    }

    public OrderFakerConfiguration SetOrderItemCount(int itemQuantity, IEnumerable<int>? orderIndexes = null)
    {
        orderIndexes ??= Enumerable.Range(0, OrderQuantity.GetValueOrDefault());

        foreach (var index in orderIndexes)
        {
            if (ItemsQuantities.ContainsKey(index))
            {
                throw new InvalidOperationException(
                    $"{nameof(itemQuantity)} for Order that has index {index} was already set"
                );
            }

            ItemsQuantities.Add(index, itemQuantity);
        }

        return this;
    }

    public OrderFakerConfiguration SetOrderItemPrice(
        decimal price,
        IEnumerable<(int OrderIndex, int OrderItemIndex)> indexes
    )
    {
        Guard.Against.Null(indexes);

        foreach (var index in indexes)
        {
            if (ItemsPrices.ContainsKey(index))
            {
                throw new InvalidOperationException(
                    $"{nameof(price)} was already set for Order {index.OrderIndex}, Order Item {index.OrderItemIndex}"
                );
            }

            ItemsPrices.Add(index, price);
        }

        return this;
    }
}
