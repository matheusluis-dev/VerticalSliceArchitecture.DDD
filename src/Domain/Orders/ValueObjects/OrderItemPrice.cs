namespace Domain.Orders.ValueObjects;

public sealed class OrderItemPrice : ValueObject
{
    public Quantity Quantity { get; init; } = null!;
    public Amount UnitPrice { get; init; } = null!;

    public Amount TotalPrice => new(Quantity.Value * UnitPrice.Value);

    public OrderItemPrice() { }

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
