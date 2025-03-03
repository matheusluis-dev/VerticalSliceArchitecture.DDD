namespace Domain.Common.ValueObjects;

public sealed class Quantity : ValueObject
{
    public int Value { get; init; }

    [UsedImplicitly]
    public Quantity() { }

    public Quantity(int value)
    {
        if (value <= 0)
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
    public QuantityException() { }

    public QuantityException(string message)
        : base(message) { }

    public QuantityException(string message, Exception innerException)
        : base(message, innerException) { }

    internal static QuantityException QuantityZeroOrLesser()
    {
        return new QuantityException("Quantity must be greater than zero");
    }
}
