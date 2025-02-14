namespace Domain.Orders.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct OrderId
{
    public static OrderId Create() => From(Guid.NewGuid());
}
