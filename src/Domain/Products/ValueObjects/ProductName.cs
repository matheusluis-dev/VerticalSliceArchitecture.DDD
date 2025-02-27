namespace Domain.Products.ValueObjects;

public sealed class ProductName : ValueObject
{
    public string Value { get; init; } = null!;

    [UsedImplicitly]
    public ProductName() { }

    public ProductName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Value = Normalize(name);
    }

    private static string Normalize(string name)
    {
        return name.ToUpperInvariant();
    }

    internal bool IsFilled()
    {
        return Value.Trim().Length > 0;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}
