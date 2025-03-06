namespace Domain.Common.ValueObjects;

public sealed class Amount : ValueObject<decimal>
{
    public Amount(decimal value)
        : base(value)
    {
        if (value < 0)
            throw AmountException.AmountZeroOrLesser();
    }
}

public sealed class AmountException : Exception
{
    public AmountException() { }

    public AmountException(string message)
        : base(message) { }

    public AmountException(string message, Exception innerException)
        : base(message, innerException) { }

    internal static AmountException AmountZeroOrLesser()
    {
        return new AmountException("Amount must be greater than zero");
    }
}
