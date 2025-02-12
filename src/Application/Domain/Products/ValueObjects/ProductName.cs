namespace Application.Domain.Products.ValueObjects;

using Vogen;

[ValueObject<string>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct ProductName
{
    public bool IsNullOrWhiteSpace()
    {
        return string.IsNullOrWhiteSpace(Value);
    }

    private static string NormalizeInput(string input)
    {
        return input.ToUpperInvariant();
    }
}
