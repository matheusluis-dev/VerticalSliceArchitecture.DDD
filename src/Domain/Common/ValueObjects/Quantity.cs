namespace Domain.Common.ValueObjects;

using System.Collections.Generic;

public sealed class Quantity : ValueObject
{
    public int Value { get; init; }

    public Quantity(int value)
    {
        if (value < 0)
            throw QuantityException.QuantityZeroOrLesser();

        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

public sealed class QuantityException : Exception
{
    private QuantityException(string message)
        : base(message) { }

    public static QuantityException QuantityZeroOrLesser()
    {
        return new QuantityException("Quantity must be greater than zero");
    }
}
