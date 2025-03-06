namespace Domain.Products.ValueObjects;

public sealed class ProductName : ValueObject<string>
{
    public ProductName(string value)
        : base(value) { }

    protected override string Normalize(string value)
    {
        return value.ToUpperInvariant();
    }
}
