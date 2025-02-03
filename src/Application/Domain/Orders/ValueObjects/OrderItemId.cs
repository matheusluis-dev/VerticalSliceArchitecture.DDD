namespace Application.Domain.Orders.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct OrderItemId
{
    public static OrderItemId Create() => From(Guid.NewGuid());
}
