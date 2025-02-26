namespace Domain.Common.ValueObjects;

public sealed class Amount : ValueObject
{
    public decimal Value { get; init; }

    public Amount() { }

    public Amount(decimal value)
    {
        if (value < 0)
            throw AmountException.AmountZeroOrLesser();

        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

public sealed class AmountException : Exception
{
    private AmountException(string message)
        : base(message) { }

    internal static AmountException AmountZeroOrLesser()
    {
        return new AmountException("Amount must be greater than zero");
    }
}
