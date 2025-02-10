namespace Application.Domain.Products.ValueObjects;

using Vogen;

[ValueObject<string>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct ProductName
{
    private static Validation Validate(string input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? Validation.Ok
            : Validation.Invalid("Product name must be defined.");
    }

    private static string NormalizeInput(string input)
    {
        return input.ToUpperInvariant();
    }
}
