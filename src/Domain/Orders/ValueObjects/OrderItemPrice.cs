namespace Domain.Orders.ValueObjects;

using System.Collections.Generic;
using Domain.Common.ValueObjects;

public sealed class OrderItemPrice : ValueObject
{
    public Quantity Quantity { get; init; }
    public Amount UnitPrice { get; init; }

    public Amount TotalPrice => new(Quantity.Value * UnitPrice.Value);

    public OrderItemPrice(int quantity, decimal unitPrice)
    {
        Quantity = new Quantity(quantity);
        UnitPrice = new Amount(unitPrice);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Quantity;
        yield return UnitPrice;
    }
}
