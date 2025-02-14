namespace Domain.Inventories.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct ReservationId
{
    public static ReservationId Create() => From(Guid.NewGuid());
}
