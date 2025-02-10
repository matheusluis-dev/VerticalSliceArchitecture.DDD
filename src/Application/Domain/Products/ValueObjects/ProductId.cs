namespace Application.Domain.Products.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct ProductId
{
    public static ProductId Create() => From(Guid.NewGuid());
}
