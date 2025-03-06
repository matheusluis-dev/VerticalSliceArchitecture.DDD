namespace Domain.Common.ValueObjects;

public sealed class Quantity : ValueObject<int>
{
    public Quantity(int value)
        : base(value)
    {
        if (value <= 0)
            throw QuantityException.QuantityZeroOrLesser();
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
