namespace Application.Domain.Common.ValueObjects;

using Vogen;

[ValueObject<decimal>]
public readonly partial struct Amount
{
    public static readonly Amount Zero = From(0);

    public Amount MultiplyByQuantity(Quantity quantity)
    {
        var quantityValue = (decimal)quantity.Value;
        var amountValue = Value;

        return From(amountValue * quantityValue);
    }
}
